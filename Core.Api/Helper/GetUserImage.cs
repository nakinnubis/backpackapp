using Microsoft.AspNetCore.Http;
using System;
using System.Drawing.Imaging;
namespace Core.Api.Helper
{


    /// <summary>
    /// Object in this class manages user image and path.
    /// </summary>
    public static class GetUserImage
    {
    
        /// <summary>
        /// Returns user image path.
        /// </summary>
        public static string ImagePathForUserPhoto
        {
            get
            {
                return "UserImages/";
                //return "myimages/Users_Photo/";
            }
        }

        public static string ImagePathForActivityTypePhoto
        {
            get
            {
                return "ActivityTypeImages/";
                //return "myimages/Users_Photo/";
            }
        }
        public static string ImagePathForActivity
        {
            get
            {
                return "myimages/";
                //return "myimages/Users_Photo/";
            }
        }
        public static string OnlineImagePathForUserPhoto
        {
            get
            {
                //change to current hosted url
                return "http://petroquestdata.com/" + ImagePathForUserPhoto;// +"/";
            }
        }

        public static string OnlineImagePathForActivityTypePhoto
        {
            get
            {
                //return Startup._hostingEnvironment.ContentRootPath + ImagePathForActivityTypePhoto;// +"/";
                return "http://petroquestdata.com/" + ImagePathForActivityTypePhoto;// +"/";
            }
        }
        public static string OnlineImagePathForActivity
        {
            get
            {
                return "http://petroquestdata.com/" + ImagePathForActivity;// +"/";
            }
        }

        private static IHttpContextAccessor HttpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public static string GetAbsoluteUri()
        {
            var request = HttpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri.ToString();
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
