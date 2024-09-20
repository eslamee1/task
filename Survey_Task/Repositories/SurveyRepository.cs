using Survey_Task.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Survey_Task.Repositories
{
    public class SurveyRepository(ApplicationDbContext context)
    {
        public async Task AddSurveyAsync(Survey survey)
        {
            await context.Surveys.AddAsync(survey);
            await context.SaveChangesAsync();
        }

        public IEnumerable<Survey> GetSurveys()
        {
            return context.Surveys.ToList();
        }
    }
}
