using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Core.Web.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }





        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Rule> Rule { get; set; }
        public virtual DbSet<Activity_Rule> Activity_Rule { get; set; }
        public virtual DbSet<IndividualCategory> Individual_Categories { get; set; }
        public virtual DbSet<Activity_Add_Ons> Activity_Add_Ons { get; set; }
        public virtual DbSet<Activity_Option> Activity_Option { get; set; }
        public virtual DbSet<Activity_Organizer> Activity_Organizer { get; set; }
        public virtual DbSet<Activity_Photos> Activity_Photos { get; set; }
        public virtual DbSet<ActivityType> ActivityType { get; set; }
        public virtual DbSet<Add_Ons> Add_Ons { get; set; }
        public virtual DbSet<Avaliability> Avaliability { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Booking_Ticket> Booking_Ticket { get; set; }
        public virtual DbSet<TicketMessage> TicketMessages { get; set; }
        public virtual DbSet<Option> Option { get; set; }
        public virtual DbSet<Organizer_Type> Organizer_Type { get; set; }
        public virtual DbSet<Reviews> Review { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Activity_Log> Activity_Log { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<Booking_individual_category_capacity> Booking_individual_category_capacity { get; set; }
        public virtual DbSet<Booking_Ticket_Addon> Booking_Ticket_Addon { get; set; }
        public virtual DbSet<Identification_types> Identification_types { get; set; }
        public virtual DbSet<Nationalities> Nationalities { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<PaymentMethods> PaymentMethods { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<User_roles> User_roles { get; set; }
        public virtual DbSet<ReviewReports> ReviewReports { get; set; }
        public virtual DbSet<Avaliability_Pricing> Avaliability_Pricings { get; set; }
        public virtual DbSet<ReviewReplies> ReviewReplies { get; set; }
        public virtual DbSet<User_Favorite_Activities> User_Favorite_Activities { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<MessageReply> MessageReplies { get; set; }
        public virtual DbSet<UserIdentification> UserIdentifications { get; set; }
        public virtual DbSet<User_Diseases> User_Diseases { get; set; }
        public virtual DbSet<Diseases> Diseases { get; set; }
        public virtual DbSet<FollowUpHealth> FollowUpHealth { get; set; }
        public virtual DbSet<UserDevice> UserDevices { get; set; }

        public AppDbContext(DeleteBehavior deleteBehavior, bool requiredRelationship)
        {
            DeleteBehavior = deleteBehavior;
            RequiredRelationship = requiredRelationship;

            if (LogMessages == null)
            {
                LogMessages = new List<string>();
                this.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            }
        }

        public DeleteBehavior DeleteBehavior { get; }
        public bool RequiredRelationship { get; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string,
                //#warning you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //"//"Persist Security Info=False;server = db781513661.hosting-data.io; Initial Catalog = db781513661; User ID = dbo781513661;Password = Developer#123"
                //"Server=dea5f48d-f5f9-4929-8e57-aa3000a3243f.sqlserver.sequelizer.com;Database=dbdea5f48df5f949298e57aa3000a3243f;User ID=oamekanmvwdglsae;Password=itqJhQN5bhxAFs6DeKQtWGBekxpUvjF4A7dSQuHZuGhqKe7E6MjXbPvHZkxRA2HZ;"
                //"Server= DESKTOP-H39SEMJ\\SQLEXPRESS;Database=db781513661;Trusted_Connection=True;"
                optionsBuilder.UseSqlServer("Persist Security Info=False;server = db781513661.hosting-data.io; Initial Catalog = db781513661; User ID = dbo781513661;Password = Developer#123");



            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            //builder.ForSqlServerUseSequenceHiLo("ticketNO");
            //builder.Entity<Booking_Ticket>()
            //.Property(o => o.ticket_number).ForSqlServerUseSequenceHiLo().ValueGeneratedNever();

            //builder.HasSequence<int>("ticketNO")
            //       .StartsAt(100000000000).IncrementsBy(1);
            //builder.ForSqlServerUseSequenceHiLo("ticketNO");

            //builder.Entity<Booking_Ticket>()
            //.Property(o => o.ticket_number).ValueGeneratedNever
            base.OnModelCreating(builder);

        }


        public class DeleteBehaviorCacheKeyFactory : IModelCacheKeyFactory
        {
            public virtual object Create(DbContext context)
            {
                var applicationDbContext = (AppDbContext)context;

                return (applicationDbContext.DeleteBehavior, applicationDbContext.RequiredRelationship);
            }
        }

        public static IList<string> LogMessages;

        private class MyLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName) => new SampleLogger();

            public void Dispose() { }

            private class SampleLogger : ILogger
            {
                public bool IsEnabled(LogLevel logLevel) => true;

                public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                    Func<TState, Exception, string> formatter)
                {
                    if (eventId.Id == RelationalEventId.CommandExecuting.Id)
                    {
                        var message = formatter(state, exception);
                        var commandIndex = Math.Max(message.IndexOf("UPDATE"), message.IndexOf("DELETE"));
                        if (commandIndex >= 0)
                        {
                            var truncatedMessage = message.Substring(commandIndex, message.IndexOf(";", commandIndex) - commandIndex).Replace(Environment.NewLine, " ");

                            for (var i = 0; i < 4; i++)
                            {
                                var paramIndex = message.IndexOf($"@p{i}='");
                                if (paramIndex >= 0)
                                {
                                    var paramValue = message.Substring(paramIndex + 5, 1);
                                    if (paramValue == "'")
                                    {
                                        paramValue = "NULL";
                                    }

                                    truncatedMessage = truncatedMessage.Replace($"@p{i}", paramValue);
                                }
                            }

                            LogMessages.Add(truncatedMessage);
                        }
                    }
                }

                public IDisposable BeginScope<TState>(TState state) => null;

            }
        }





    }
}
