using huhu.IDAL;
using System.Data.Entity;


namespace huhu.DalFactory
{
    public class DbSession
    {
        public DbContext GetContext()
        {
            return DbContextFactory.GetCurrentDbContext();
        }

        public IArticleDAL ArticleDal { get; set; }
        public IArticleTagDAL ArticleTagDal { get; set; }
        public IUserDAL UserDal { get; set; }
        public IAreaCodeDAL AreaCodeDal { get; set; }
        public IAdvertDAL AdvertDal { get; set; }
        public IArticleDraftDAL ArticleDraftDal { get; set; }
        public IDiggDAL DiggDal { get; set; }
        public IArticleViewDAL ArticleViewDal { get; set; }
        public IArticleCommentDAL ArticleCommentDal { get; set; }
        public IArticleReplyDAL ArticleReplyDal { get; set; }
        public IFollowDAL FollowDal { get; set; }
        public IUserCollectionDAL UserCollectionDal { get; set; }
        public IUserCollectDAL UserCollectDal { get; set; }
        public IFeedBackDAL FeedBackDal { get; set; }
        public IAdminDAL AdminDal { get; set; }
        public IAdminManagerDAL AdminManagerDal { get; set; }
        public ITopicDAL TopicDal { get; set; }
        public ITopicCircleDAL TopicCircleDal { get; set; }
        public IReportOptionDAL ReportOptionDal { get; set; }
        public IReportDAL ReportDal { get; set; }
        public IUserLocateDAL UserLocateDal { get; set; }


        public int SaveChanges()
        {
            return DbContextFactory.GetCurrentDbContext().SaveChanges();
        }
    }
}
