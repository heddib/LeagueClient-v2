using System;
using System.Collections.Generic;
using Kappa.Settings;
using MFroehlich.Parsing.JSON;

namespace Kappa.BackEnd.Server.Meta {
    [Docs("group", "Meta")]
    public class SettingsService : JSONService {
        private Settings settings;
        private Session session;

        public SettingsService(Session session) : base("/meta/settings") {
            this.session = session;
            this.session.Authed += Session_Authed;
            settings = GetSettings<Settings>();
            settings.UserSettings = settings.UserSettings ?? new Dictionary<string, JSONObject>();
        }

        private void Session_Authed(object sender, EventArgs eventArgs) {
            if (!settings.UserSettings.ContainsKey(this.session.Me.Username)) {
                settings.UserSettings[this.session.Me.Username] = new JSONObject();
            }
        }

        [Endpoint("/user")]
        public JSONObject GetUserSettings() {
            return settings.UserSettings[session.Me.Username];
        }

        [Endpoint("/user/patch")]
        public void PatchUserSettings(JSONObject now) {
            var user = settings.UserSettings[session.Me.Username];
            foreach (var pair in now) {
                user[pair.Key] = pair.Value;
            }
            this.settings.Save();
        }

        [Endpoint("/user/delete")]
        public void DeleteUserSettings(string key) {
            var user = settings.UserSettings[session.Me.Username];
            user.Remove(key);
            this.settings.Save();
        }

        private class Settings : BaseSettings {
            public Dictionary<string, JSONObject> UserSettings { get; set; }
        }
    }
}
