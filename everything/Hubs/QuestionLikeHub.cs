using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using everything.DataLayer;
using everything.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Data.Entity;

namespace everything.Hubs
{
    [HubName("questionLikeHub")]
    public class QuestionLikeHub : Hub
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        public Task Like(int QuestionId,string UserId)
        {
            var likePost = SaveLike(QuestionId,UserId);
            return Clients.All.updateLikeCount(likePost);
        }

        private LikeQuestion SaveLike(int id,string UserId)
        {
            var QuestionId = id;
            var baseContext = Context.Request.GetHttpContext();
            var item = _applicationDbContext.Questions.SingleOrDefault(i => i.QuestionId == QuestionId);
            
            var liked = new QuestionLike
            {
                QuestionId = item.QuestionId,
                IPAddress = baseContext.Request.UserHostAddress,
                UserAgent = baseContext.Request.UserAgent,
                UserId = UserId,
                UserLike = true
            };
            var getUserLike = item.QuestionLikes.FirstOrDefault(e => e.UserId == liked.UserId);
            if (getUserLike == null)
            {
                _applicationDbContext.QuestionLikes.Add(liked);

            }
            else
            {
                getUserLike.UserLike = !getUserLike.UserLike;
            }

                _applicationDbContext.SaveChangesAsync();
    
            var question = _applicationDbContext.Questions.SingleOrDefault(i => i.QuestionId == QuestionId);
            var UserStatus = _applicationDbContext.QuestionLikes.SingleOrDefault(i => i.QuestionId == QuestionId && i.UserId == UserId);

            return new LikeQuestion
            {
                LikeCount = question.QuestionLikes.Count(e => e.UserLike),
                UserLikeStatus = UserStatus.UserLike

            };
        }
    }
}