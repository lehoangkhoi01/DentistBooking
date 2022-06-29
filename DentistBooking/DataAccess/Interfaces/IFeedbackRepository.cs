using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IFeedbackRepository
    {
        public IEnumerable<Feedback> GetFeedbacks();
        public void AddNewFeedback(Feedback feedback);
        public void UpdateFeedback(Feedback feedback);
        public void DeleteFeedback(Feedback feedback);
    }
}
