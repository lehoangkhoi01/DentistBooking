using BusinessObject;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class FeedbackDAO
    {
        private static FeedbackDAO instance = null;
        private static readonly object instanceLock = new object();

        private FeedbackDAO() { }
        public static FeedbackDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FeedbackDAO();
                    }
                }
                return instance;
            }
        }
        //----------------------------------------------------------

        public void AddFeedback(Feedback feedback)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Feedbacks.Add(feedback);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateFeedback(Feedback feedback)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Entry<Feedback>(feedback).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteFeedback(Feedback feedback)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Feedbacks.Remove(feedback);
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Feedback> GetFeedbacks()
        {
            IEnumerable<Feedback> feedbacks;
            try
            {
                var dbContext = new DentistBookingContext();
                feedbacks = dbContext.Feedbacks.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return feedbacks;
        }
    }
}
