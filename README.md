# Ikst.ImageUtil
This is a utility class to simplify image-related operations such as image resizing and file output.
It is mainly provided as an extension method of the Image class.

The following resize modes can be specified for resizing images.

- None  
    Keeps the current size of the content.

- Fill  
    Resizes the content to fill the allotted space. The aspect ratio will not be maintained.

- Uniform   
    Resizes the content to fit in the allotted area, but maintains the original aspect ratio.

- UniformToFill (default)  
    Resizes the content to fill the allotted space, but preserves the original aspect ratio. If the aspect ratio of the source content is different from the aspect ratio of the target rectangle, the source content will be cropped to fit into the target rectangle.

- FixedAspectRatioResize  
    Resizes the image while maintaining the aspect ratio before and after the transformation.
    The size will not be the same as the specified size.


## usege
Download an image, resize it, and save it.
The image file format is determined from the file name extension to determine the correct format.

```C#
using Ikst.ImageUtil;

public void eg()
{
    Image img = ImageUtil.DownloadImage("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png").Result;
    Image resizeImg = img.Resize(90, 30, ResizeMode.Uniform);

    img.SaveFile("result1.png");
    resizeImg.SaveFile("result2.png");

    img.Dispose();
    resizeImg.Dispose();
}
```

## nuget
https://www.nuget.org/packages/Ikst.ImageUtil/
