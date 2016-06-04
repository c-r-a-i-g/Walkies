using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using Walkies.Core.Enumerations;
using Walkies.Framework.Breadcrumbs;
using Walkies.Framework.Cookies;
using Walkies.Framework.Enumerations;
using Walkies.Framework.Notifications;

namespace Walkies.Framework.Interfaces
{

    public interface IPageModel
    {
        void Notify( string content, NotificationType notificationType = NotificationType.Information );
        string GetBreadcrumbTitle();
        void AddData( string key, object value );

        List<Breadcrumb> Breadcrumbs { get; }
        List<Notification> Notifications { get; set; }
        Sidebar Sidebar { get; set; }
        Background Background { get; set; }
        bool UpdateSidebarIfPageKeyChanges { get; set; }
        string PageType { get; set; }
        string PageTitle { get; set; }
        string PageSubTitle { get; set; }
        string PageIcon { get; set; }
        string PageKey { get; set; }
        string EncodedPageData { get; }
        string BackUrl { get; }
        bool IsRefresh { get; }
        bool IsAjax { get; }
        bool IsFullScreen { get; }
        bool IsUsingMainScrollbar { get; set; }
        bool IsPinnable { get; set; }
        WalkiesCookie Cookie { get; }
        string PageController { get; }
        string PageAction { get; }
        PageHeaderType PageHeaderType { get; set; }

    }

}
