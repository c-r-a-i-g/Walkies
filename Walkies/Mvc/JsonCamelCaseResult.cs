using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Web.Mvc;

namespace Walkies.Mvc
{
    /// <summary>
    /// Used to return a JsonResult that will convert all properties to camelCase.
    /// Uses Newtonsoft to do the conversion.
    /// </summary>
    /// <remarks>
    /// This was obtained from StackOverflow. All comments and formatting are created by Barnesy.
    /// </remarks>
    public class JsonCamelCaseResult : ActionResult
    {

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Class Members

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Constructor & Intialisation

        public JsonCamelCaseResult( object data, JsonRequestBehavior jsonRequestBehavior )
        {
            Data = data;
            JsonRequestBehavior = jsonRequestBehavior;
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Private Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Protected Methods

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Public Methods

        /// <summary>
        /// Same as the JsonResult class, except the conversion is done via Newtonsoft and uses the Camel Case Contract Resolver.
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult( ControllerContext context )
        {
            if ( context == null )
            {
                throw new ArgumentNullException( "context" );
            }
            if ( JsonRequestBehavior == JsonRequestBehavior.DenyGet && String.Equals( context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase ) )
            {
                throw new InvalidOperationException( "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet." );
            }

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty( ContentType ) ? ContentType : "application/json";
            if ( ContentEncoding != null )
            {
                response.ContentEncoding = ContentEncoding;
            }
            if ( Data == null )
                return;

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            response.Write( JsonConvert.SerializeObject( Data, jsonSerializerSettings ) );
        }

        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        #region Properties

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonRequestBehavior JsonRequestBehavior { get; set; }
        
        #endregion

        /* ----------------------------------------------------------------------------------------------------------------------------------------- */

        
    }
}
