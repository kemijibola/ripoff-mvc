using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using everything.Models;
using System.Web.Mvc;

namespace everything.ViewModels
{
    public class CreateAnswerViewModel
    {
        public string AnswerText { get; set; }
        public string Comment { get; set; }
        public int AnswerToQuestionId { get; set; }

    }

    public class AllQuestionListViewModel
    {
        public PagedList.IPagedList<QuestionListWithOwnerAndLastParticipantViewModel> QuestionListWithOwnerAndLastParticipantViewModels { get; set; }
        public virtual List<TopSevenQuestions> TopSevenQuestions { get; set; }
        public virtual List<TopTenReport> TopTenReports { get; set; }
        public virtual List<MostLikeQuestion> MostLikeQuestions { get; set; }
    }
    public class QuestionListWithOwnerAndLastParticipantViewModel
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int AnswerCount { get; set; }
        public string QuestionOwner { get; set; }
        public string DiscussionCategory { get; set; }
        public string DateAsked { get; set; }
        public string LastParticipantDate { get; set; }
        public string LastParticipantOwner { get; set; }
    }
    public class FullQuestionViewModel
    {
        //this handles form submit for answer question
        [AllowHtml]
        public string Answer { get; set; }
        public int QuestionId { get; set; }
        public int Count { get; set; }
        public int QuestionLikes { get; set; }
        public string CurrentUser { get; set; }
        public bool hasUserLiked { get; set; }
        //.........................................
        public string Title { get; set; }
        public string QuestionText { get; set; }
        public string QuestionOwner { get; set; }
        public string QuestionOwnerDP { get; set; }
        public string Discussion { get; set; }
        public string TimeAsked { get; set; }
        public string DateAsked { get; set; }
        public string LastActiveDate { get; set; }
        public string LastActiveTime { get; set; }
        public virtual List<AnswerToQuestionViewModel> AnswerToQuestionViewModels { get; set; }
        public virtual List<AnswerCommentViewModel> AnswerCommentViewModels { get; set; }
        public virtual List<RelatedQuestionViewModel> RelatedQuestionViewModels { get; set; }

    }

    public class RelatedQuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTitle { get; set; }
        
    }
    public class QuestionCommentViewModel
    {
        public string QuestionComment { get; set; }
        public string QCommentOwner { get; set; }
        public DateTime QCDateCommented { get; set; }
    }

    public class AnswerCommentViewModel
    {
        public int AnswerToQuestionId { get; set; }
        public string AComment { get; set; }
        public string ACommentOwner { get; set; }
        public string ACTimeCommented { get; set; }
        public string ACommentOnwerDP { get; set; }
        public string ACDateCommented { get; set; }

    }
    public class AnswerToQuestionViewModel
    {
        public int QuestionId { get; set; }
        public int AnswerToQuestionId { get; set; }
        public string AToQuestionOwner { get; set; }
        public string AToQuestionOwnerDP { get; set; }
        public string AToQuestion { get; set; }
        public string AToTimeCommented { get; set; }
        public string AToCDateCommented { get; set; }
    }
}
