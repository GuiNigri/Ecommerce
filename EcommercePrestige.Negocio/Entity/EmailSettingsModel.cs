namespace EcommercePrestige.Model.Entity
{
    public class EmailSettingsModel
    {
        public string PrimaryDomain { get; set; }
        public int PrimaryPort { get; set; }
        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }

        public EmailSettingsModel()
        {
        }

        public EmailSettingsModel(string primaryDomain, int primaryPort, string usernameEmail, string usernamePassword)
        {
            PrimaryDomain = primaryDomain;
            PrimaryPort = primaryPort;
            UsernameEmail = usernameEmail;
            UsernamePassword = usernamePassword;
        }
    }
}
