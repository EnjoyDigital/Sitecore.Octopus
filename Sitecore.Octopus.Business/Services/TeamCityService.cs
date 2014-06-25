using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCitySharp;
using TeamCitySharp.Locators;

namespace Sitecore.Octopus.Business.Services
{
    public class TeamCityService
    {
        public TeamCityService()
        {
            var client = new TeamCityClient("localhost:81");
            client.Connect("admin", "qwerty");
            //var projects = client.Builds.ByBuildLocator(new BuildLocator(){})

        }
    }
}
