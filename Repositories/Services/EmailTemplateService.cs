namespace TestToken.Repositories.Services
{
    public class EmailTemplateService
    {
        private readonly string _templatePath;

        public EmailTemplateService(string templatePath)
        {
            _templatePath = templatePath;
        }

        public string RenderWelcomeEmail(string userName, string email, string role)
            {

                var fullTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Templates", "WelcomeEmailTemplate.html");

                Console.WriteLine($"Looking for template at: {fullTemplatePath}");

                if (!File.Exists(fullTemplatePath))
                {
                    throw new FileNotFoundException("Email template file not found.", fullTemplatePath);
                }
                var template = File.ReadAllText(fullTemplatePath);
                template = template.Replace("{UserName}", userName);

                template = template.Replace("{Email}", email);
                template = template.Replace("{Role}", role);

                return template;
            }
        }
    }

