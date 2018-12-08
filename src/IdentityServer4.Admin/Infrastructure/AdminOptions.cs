using System;
using System.Collections.Generic;

namespace IdentityServer4.Admin.Infrastructure
{
    public class AdminOptions
    {
        public string ConnectionString { get; set; }
        public bool AllowLocalLogin { get; set; } = true;
        public bool AllowRememberLogin { get; set; } = true;
        public TimeSpan RememberMeLoginDuration { get; set; } = TimeSpan.FromDays(30);

        public bool ShowLogoutPrompt { get; set; } = true;
        public bool AutomaticRedirectAfterSignOut { get; set; } = true;

        // specify the Windows authentication scheme being used
        public string WindowsAuthenticationSchemeName { get; set; } =
            Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        // if user uses windows auth, should we load the groups from windows
        public bool IncludeWindowsGroups { get; set; } = false;

        public string InvalidCredentialsErrorMessage { get; set; } = "用户名或密码错误";

        public HashSet<string> Group { get; set; }
        public HashSet<string> Title { get; set; }
        public HashSet<string> Level { get; set; }
    }
}