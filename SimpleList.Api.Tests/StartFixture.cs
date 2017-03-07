using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using SimpleList.Api.Models;

namespace SimpleList.Api.Tests
{
    public class StartFixture<TStartup, TestTarget> : IDisposable
    {
        private const string SolutionName = "SimpleList.sln";
        public TestServer Server { get; }
        public HttpClient Client { get; }
        public string AuthorizationCookie { get; }

        public StartFixture() : this(string.Empty)
        {
        }

        protected StartFixture(string solutionRelativeTargetProjectParentDir)
        {
            var startupAssembly = typeof(TestTarget).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(solutionRelativeTargetProjectParentDir, startupAssembly);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .UseStartup(typeof(TStartup));

            Server = new TestServer(builder);

            Client = Server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost");

            var login = new CredentialModel
            {
                User = "testuser",
                Password = "P@ssw0rd!"
            };

            var response = Client.PostAsJsonAsync("api/auth/standartlogin", login).Result;
            response.EnsureSuccessStatusCode();

            var cookie = response.Headers.Where(r => r.Key == "Set-Cookie").First().Value.First();
            var parsed = cookie.Split(new[] { ';', ',', '=' },
                              StringSplitOptions.RemoveEmptyEntries);

            AuthorizationCookie = cookie;
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}
