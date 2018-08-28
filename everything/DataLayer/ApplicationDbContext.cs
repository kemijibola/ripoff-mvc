using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using everything.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace everything.DataLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("EverythingContext", throwIfV1Schema: false)
        {
        }


        public DbSet<Bank> Banks { get; set; }
        public DbSet<CaseUpdate> CaseUpdates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ClientLawsuit> ClientLawsuits { get; set; }
        public DbSet<ClientMeetingRequestTemp> ClientMeetingRequestTemps { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FirmRegion> FirmRegions { get; set; }
        public DbSet<Lawfirm> Lawfirms { get; set; }
        public DbSet<Rebuttal> Rebuttals { get; set; }
        public DbSet<RebuttalImage> RebuttalImages { get; set; }
        public DbSet<RejectionReason> RejectionReasons { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportBug> ReportBugs { get; set; }
        public DbSet<ReportImage> ReportImages { get; set; }
        public DbSet<ReportRejection> ReportRejections { get; set; }
        public DbSet<ReportUpdate> ReportUpdates { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ReportVideo>  ReportVideos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RipOffLegalTeamAdvice> RipOffLegalTeamAdvices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionComment> QuestionComments { get; set; }
        public DbSet<AnswerToQuestion> AnswerToQuestions { get; set; }
        public DbSet<AnswerComment> AnswerComments { get; set; }
        public DbSet<DiscussionCategory> DiscussionCategories { get; set; }
        public DbSet<UserProfilePhoto> UserProfilePhotos { get; set; }
        public DbSet<LogFalsePasswordReset> LogFalsePasswordResets { get; set; }
        public DbSet<LawyerRequest> LawyerRequests { get; set; }
        public DbSet<ApprovedLawyerRequest> ApprovedLawyerRequests { get; set; }
        public DbSet<RejectedLawyerRequest> RejectedLawyerRequests { get; set; }
        public DbSet<LawyerRejectionReason> LawyerRejectionReason { get; set; }
        public DbSet<RipoffCaseUpdate> RipoffCaseUpdates { get; set; }
        public DbSet<RCaseUpdateImage> RCaseUpdateImages { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<QuestionVote> QuestionVotes { get; set; }
        public DbSet<QuestionLike> QuestionLikes { get; set; }
        public DbSet<UpdateAdvice> UpdateAdvices { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<BlogVideo> BlogVideos { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BankConfiguration());
            modelBuilder.Configurations.Add(new ApplicationUserConfiguration());
            modelBuilder.Configurations.Add(new CaseUpdateConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new CityConfiguration());
            modelBuilder.Configurations.Add(new ClientLawsuitConfiguration());
            modelBuilder.Configurations.Add(new ClientMeetingRequestTempConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new FirmRegionConfiguration());
            modelBuilder.Configurations.Add(new LawfirmConfiguration());
            modelBuilder.Configurations.Add(new RebuttalConfiguration());
            modelBuilder.Configurations.Add(new RejectionReasonConfiguration());
            modelBuilder.Configurations.Add(new ReportBugConfiguration());
            modelBuilder.Configurations.Add(new ReportConfiguration());
            modelBuilder.Configurations.Add(new ReportImageConfiguration());
            modelBuilder.Configurations.Add(new ReportRejectionConfiguration());
            modelBuilder.Configurations.Add(new ReportUpdateConfiguration());
            modelBuilder.Configurations.Add(new StateConfiguration());
            modelBuilder.Configurations.Add(new ThreadConfiguration());
            modelBuilder.Configurations.Add(new TopicConfiguration());
            modelBuilder.Configurations.Add(new ReportVideoConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
            modelBuilder.Configurations.Add(new RipOffLegalTeamAdviceConfiguration());
            modelBuilder.Configurations.Add(new QuestionConfiguration());
            modelBuilder.Configurations.Add(new QuestionCommentConfiguration());
            modelBuilder.Configurations.Add(new AnswerCommentConfiguration());
            modelBuilder.Configurations.Add(new AnswerToQuestionConfiguration());
            modelBuilder.Configurations.Add(new DiscussionCategoryConfiguration());
            modelBuilder.Configurations.Add(new UserProfilePhotoConfiguration());
            modelBuilder.Configurations.Add(new LogFalsePasswordResetConfiguration());
            modelBuilder.Configurations.Add(new LawyerRequestConfiguration());
            modelBuilder.Configurations.Add(new ApprovedLawyerRequestConfiguration());
            modelBuilder.Configurations.Add(new RejectedLawyerRequestConfiguration());
            modelBuilder.Configurations.Add(new LawyerRejectionReasonConfiguration());
            modelBuilder.Configurations.Add(new RipoffCaseUpdateConfiguration());
            modelBuilder.Configurations.Add(new RCaseUpdateImageConfiguration());
            modelBuilder.Configurations.Add(new SocialMediaConfiguration());
            modelBuilder.Configurations.Add(new QuestionVoteConfiguration());
            modelBuilder.Configurations.Add(new QuestionLikeConfiguration());
            modelBuilder.Configurations.Add(new UpdateAdviceConfiguration());
            modelBuilder.Configurations.Add(new BlogConfiguration());
            modelBuilder.Configurations.Add(new BlogImageConfiguration());
            modelBuilder.Configurations.Add(new BlogVideoConfiguration());
            modelBuilder.Configurations.Add(new BlogCommentConfiguration());


            base.OnModelCreating(modelBuilder);
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}