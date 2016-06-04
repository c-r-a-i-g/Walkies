using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Postal;

using Walkies.Framework.Web;
using Walkies.Framework.Interfaces;
using Walkies.Framework.Web.Session;
using Walkies.Database;
using Walkies.Database.Entities;
using Walkies.Core.Configuration;

namespace Walkies.Framework.BaseClasses
{
    public class EmailModelBase : Email
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public EmailModelBase( string viewName ) : base( viewName ) 
        {

            var db = new WalkiesDB();
            var domain = HttpContext.Current.Request.Url.Authority;

            this.NormalisedViewName = this.NormaliseViewName( viewName );
            this.Theme = UserSession.Current.Theme.ToLower();
            this.ApplicationName = "Walkies";
            this.FromAddress = ApplicationSettings.Current.Email.FromAddress;
            this.FromName = ApplicationSettings.Current.Email.FromName;
            this.Url = "http://" + domain;

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Sends the email to the specified recipient
        /// </summary>
        /// <param name="toAddress">The email address of the recipient</param>
        /// <param name="toName">The full name of the recipient</param>
        public void SendTo( string toAddress, string toName )
        {
            this.ToAddress = toAddress;
            this.ToName = toName;
            this.CreateAttachments();
            this.Send();
        }

        /// <summary>
        /// Sends the email to the specified recipient
        /// </summary>
        /// <param name="user">The user to send the email to</param>
        public void SendTo( User user )
        {
            this.ToAddress = user.Email;
            this.ToName = user.FullName;
            this.CreateAttachments();
            this.Send();
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Static Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        /// <summary>
        /// Creates the attachments for the email if there are any in the theme folder
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="viewName"></param>
        private void CreateAttachments()
        {

            var server = HttpContext.Current.Server;

            try
            {

                this.Attachments = this.Attachments ?? new List<Attachment>();

                var path = server.MapPath( string.Format( "~/content/_themes/{0}/emails/{1}", this.Theme, this.NormalisedViewName ) );

                if( Directory.Exists( path ) == false ) return;

                var files = Directory.EnumerateFiles( path, "*.*", SearchOption.TopDirectoryOnly );
                foreach( var file in files )
                {
                    var attachment = new Attachment( file );
                    this.Attachments.Add( attachment );
                }

            }

            catch( Exception ) { }

        }

        /// <summary>
        /// Takes a partial view name, which may be fully qualified or in shorthand, and normalises it so that
        /// only the name of the file is returned, without extension or folders
        /// </summary>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        private string NormaliseViewName( string viewName )
        {

            var name = viewName.ToLower();
            name = name.Replace( ".cshtml", string.Empty );

            int startIndex = -1;
            if( name.Contains( "/" ) )
            {
                startIndex = name.LastIndexOf( '/' );
            }

            return name.From( startIndex + 1 );

        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public string NormalisedViewName { get; set; }
        public string Theme { get; set; }
        public string Url { get; set; }
        public string ApplicationName { get; set; }
        public string ToAddress { get; set; }
        public string ToName { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Derived Properties

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

    }
}
