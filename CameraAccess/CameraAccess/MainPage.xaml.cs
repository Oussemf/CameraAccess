using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace CameraAccess
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            takePhoto.Clicked += async (sender, args) =>
            {
                if (!IsCameraOK())
                {
                    await DisplayAlert("Camera unavailable", "Your camera is not available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Test",
                    Name = DateTime.Now.ToString() + ".jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };

            pickPhoto.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photo pick unsupported", "Error : check permissions.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });


                if (file == null)
                    return;

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            };

            takeVideo.Clicked += async (sender, args) =>
            {
                if (!IsCameraOK())
                {
                    await DisplayAlert("Camera unavailable", "Your camera is not available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = DateTime.Now.ToString() + ".mp4",
                    Directory = "DefaultVideos",
                });

                if (file == null)
                    return;

                await DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

                file.Dispose();
            };

            pickVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickVideoSupported)
                {
                    await DisplayAlert("Videos Not Supported", "Throw your phone", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickVideoAsync();

                if (file == null)
                    return;

                await DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
                file.Dispose();
            };
        }

        public bool IsCameraOK() => (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported);
    }
    
}