using Application.Core.Validators.Interfaces;
using Microsoft.AspNetCore.Hosting;
using ProfanityFilter.Interfaces;


namespace Application.Core.Validators.Implementation
{
    public class MessageProfanityValidator : IMessageProfanityValidator
    {
        private readonly IProfanityFilter _profanityFilter;
        private readonly IWebHostEnvironment _env;

        public MessageProfanityValidator(IWebHostEnvironment env)
        {
            _env = env;
            _profanityFilter = ConfigureProfanityFilter();
        }

        private IProfanityFilter ConfigureProfanityFilter()
        {
            var profanityFilter = new ProfanityFilter.ProfanityFilter();
            var basePath = Path.Combine(_env.WebRootPath);

            var files = new[]
            {
            Path.Combine(basePath, "profanity_en.txt"),
            Path.Combine(basePath, "profanity_pl.txt")
        };

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    throw new FileNotFoundException("Profanity file not found", file);

                foreach (var word in File.ReadLines(file))
                {
                    profanityFilter.AddProfanity(word);
                }
            }

            return profanityFilter;
        }

        public string Censore(string message)
        {
            return _profanityFilter.CensorString(message);
        }
    }
}
