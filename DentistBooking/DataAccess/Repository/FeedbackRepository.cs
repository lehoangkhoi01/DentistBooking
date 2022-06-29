using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public void AddNewFeedback(Feedback feedback) => FeedbackDAO.Instance.AddFeedback(feedback);

        public void DeleteFeedback(Feedback feedback) => FeedbackDAO.Instance.DeleteFeedback(feedback);

        public IEnumerable<Feedback> GetFeedbacks() => FeedbackDAO.Instance.GetFeedbacks();

        public void UpdateFeedback(Feedback feedback) => FeedbackDAO.Instance.UpdateFeedback(feedback);
    }
}
