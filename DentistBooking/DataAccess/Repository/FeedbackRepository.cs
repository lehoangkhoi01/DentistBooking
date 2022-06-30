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

        public Feedback GetFeedbackById(int id) => FeedbackDAO.Instance.GetFeedbackById(id);

        public Feedback GetFeedbackByReservationId(int id) => FeedbackDAO.Instance.GetFeedbackByReservationId(id);

        public IEnumerable<Feedback> GetFeedbacks() => FeedbackDAO.Instance.GetFeedbacks();

        public IEnumerable<Feedback> GetFeedbacksByServiceId(int id) => FeedbackDAO.Instance.GetFeedbackByServiceId(id);

        public void UpdateFeedback(Feedback feedback) => FeedbackDAO.Instance.UpdateFeedback(feedback);
    }
}
