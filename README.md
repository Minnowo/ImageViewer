# ImageViewer
  
A relatively full featured image viewer to replace the default windows image viewer  

- tab pages (1 per image)  
- drag and drop -> (opens image in new tab)  
- drag and drop -> (dragging the toolbar lets you drag the current image assuming the file exists)  
- singleinstance -> (can run with multiple instance)  
- fully customizable hotkeys -> (only within the application not system wide)  
- can dither images -> (apply your own color palette / use 3 builtin, supports [.txt, .aco, .lbm, .bmm] the .txt is the only palette i'm confident works (1 color per line, rgb))  
- invert / greyscale  
- crop / resize  
- zoom in / out  
- rotate / flip  
- undo / redo -> (it's not the greatest implementation but it does work)  
- gif frames export as / pause and unpause gif animations  
- rename, delete, move to  
- image properties  
- custom color picker -> (supports hsb, hsl, rgb, cmyk, decimal / hex, has an empty button where the screen color picker was gonna go but i didn't add that)  
- multiple settings profiles -> (all settings stored in usrConfig.xml in the same directory as the program exe)  
  
Readable image formats:  
 - png  
 - jpg, jpeg, jpe...  
 - bmp  
 - gif  
 - tif / tiff  
 - ico  
 - webp -> (no animation support & requires ./plugins/libwebp_x64.dll or ./plugins/libwebp_x32.dll)  
 - wrm / dwrm -> (this is a image format made by me as a proof of concept, its very lossy and more of an image filter but still pretty cool)  

Writeable image formats:  
 - png  
 - jpg, jpeg, jpe...  
 - bmp  
 - gif  -> (has crappy compression / doesn't use any quantizers)  
 - tif / tiff  
 - webp -> (no animation)  
 - wrm / dwrm  -> (wrm for the image to look similar to the original but with washed out color, dwrm to basically deep fry it / makes it look demonic)  

if you run this from cli `-n` spawns a new instance, every other argument should be a file path to open  

