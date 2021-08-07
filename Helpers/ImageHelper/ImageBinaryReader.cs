using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageViewer.Helpers
{
    public static class ImageBinaryReader
    {
        const byte MAX_MAGIC_BYTE_LENGTH = 8;

        public static readonly byte[] BMP_IDENTIFIER = new byte[2] { 0x42, 0x4D };
        public static readonly byte[] JPG_IDENTIFIER = new byte[3] { 0xFF, 0xD8, 0xFF };
        public static readonly byte[] TIFF_LE_IDENTIFIER = new byte[3] { 0x49, 0x49, 0x2A };
        public static readonly byte[] WEBP_IDENTIFIER = new byte[4] { 0x52, 0x49, 0x46, 0x46 };
        public static readonly byte[] TIFF_BE_IDENTIFIER = new byte[4] { 0x4D, 0x4D, 0x00, 0x2A };
        public static readonly byte[] GIF_IDENTIFIER_1 = new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };
        public static readonly byte[] GIF_IDENTIFIER_2 = new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };
        public static readonly byte[] PNG_IDENTIFIER = new byte[8] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        public static readonly byte[] WRM_IDENTIFIER = WORM.WRM_IDENTIFIER;
        public static readonly byte[] DWRM_IDENTIFIER = WORM.DWRM_IDENTIFIER;

        public static readonly Dictionary<byte[], ImgFormat> Image_Byte_Identifiers = new Dictionary<byte[], ImgFormat>()
        {
            { PNG_IDENTIFIER, ImgFormat.png },
            { JPG_IDENTIFIER, ImgFormat.jpg },
            { WEBP_IDENTIFIER, ImgFormat.webp },
            { GIF_IDENTIFIER_1, ImgFormat.gif },
            { GIF_IDENTIFIER_2, ImgFormat.gif },
            { TIFF_BE_IDENTIFIER, ImgFormat.tif },
            { TIFF_LE_IDENTIFIER, ImgFormat.tif },
            { BMP_IDENTIFIER, ImgFormat.bmp },
            { WRM_IDENTIFIER, ImgFormat.wrm },
            { DWRM_IDENTIFIER, ImgFormat.wrm }
        };

        static readonly Dictionary<byte[], Func<BinaryReader, Size>> Image_Format_Decoders = new Dictionary<byte[], Func<BinaryReader, Size>>()
        {
            { BMP_IDENTIFIER, DecodeBitmap },
            { GIF_IDENTIFIER_1, DecodeGif },
            { GIF_IDENTIFIER_2, DecodeGif },
            { PNG_IDENTIFIER, DecodePng },
            { JPG_IDENTIFIER, DecodeJfif },
            { WEBP_IDENTIFIER, DecodeWebP },
            { TIFF_LE_IDENTIFIER,  DecodeTiffLE }, // little endian
            { TIFF_BE_IDENTIFIER,  DecodeTiffBE },  // big endian
            { WRM_IDENTIFIER, DecodeWORM },
            { DWRM_IDENTIFIER, DecodeDWORM }
        };



        #region Get / Read Image Format 

        /// <summary>
        /// Gets the image mime type from the leading byte identifiers.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns>The mime type as a string.</returns>
        public static string GetMimeType(string path)
        {
            switch (GetImageFormat(path))
            {
                case ImgFormat.png:
                    return "image/png";
                case ImgFormat.jpg:
                    return "image/jpg";
                case ImgFormat.tif:
                    return "image/tiff";
                case ImgFormat.bmp:
                    return "iamge/bmp";
                case ImgFormat.gif:
                    return Gif.MimeType;
                case ImgFormat.webp:
                    return Webp.MIME_TYPE;
                case ImgFormat.wrm:
                    return WORM.MimeType;
            }
            return "image/unknown";
        }

        /// <summary>
        /// Gets the image format from the leading byte identifiers.
        /// </summary>
        /// <param name="path">The path to the image file.</param>
        /// <returns>The image format.</returns>
        public static ImgFormat GetImageFormat(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return ImgFormat.nil;


            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    byte[] magicBytes = new byte[MAX_MAGIC_BYTE_LENGTH];

                    for (int i = 0; i < MAX_MAGIC_BYTE_LENGTH; i += 1)
                    {
                        magicBytes[i] = binaryReader.ReadByte();

                        foreach (KeyValuePair<byte[], ImgFormat> kvPair in Image_Byte_Identifiers)
                        {
                            if (ByteHelper.StartsWith(magicBytes, kvPair.Key))
                            {
                                return kvPair.Value;
                            }
                        }
                    }
                    return ImgFormat.nil;
                }
            }
            catch
            {
                return ImgFormat.nil;
            }
        }


        #endregion


        #region Get / Read Image Dimensions 


        /// <summary>        
        /// Gets the dimensions of an image.        
        /// </summary>        
        /// <param name="path">The path of the image to get the dimensions of.</param>        
        /// <returns>The dimensions of the specified image.</returns>        
        /// <exception cref="ArgumentException">The image was of an unrecognised format.</exception>            
        public static Size GetDimensions(BinaryReader binaryReader)
        {
            byte[] magicBytes = new byte[MAX_MAGIC_BYTE_LENGTH];

            for (int i = 0; i < MAX_MAGIC_BYTE_LENGTH; i += 1)
            {
                magicBytes[i] = binaryReader.ReadByte();

                foreach (KeyValuePair<byte[], Func<BinaryReader, Size>> kvPair in Image_Format_Decoders)
                {
                    if (ByteHelper.StartsWith(magicBytes, kvPair.Key))
                    {
                        return kvPair.Value(binaryReader);
                    }
                }
            }

            return Size.Empty;
        }

        /// <summary>
        /// Gets the dimensions of an image.
        /// </summary>
        /// <param name="path">The path of the image to get the dimensions of.</param>
        /// <returns>The dimensions of the specified image.</returns>
        /// <exception cref="ArgumentException">The image was of an unrecognized format.</exception>
        public static Size GetDimensions(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return Size.Empty;

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    return GetDimensions(binaryReader);
                }
            }
            catch
            {
                return Size.Empty;
            }
        }

        private static Size DecodeWORM(BinaryReader binaryReader)
        {
            return WORM.GetDimensions(binaryReader);
        }

        private static Size DecodeDWORM(BinaryReader binaryReader)
        {
            return WORM.GetDimensions(binaryReader);
        }

        private static Size DecodeTiffLE(BinaryReader binaryReader)
        {
            if (binaryReader.ReadByte() != 0)
                return Size.Empty;

            int idfStart = ByteHelper.ReadInt32LE(binaryReader);

            binaryReader.BaseStream.Seek(idfStart, SeekOrigin.Begin);

            int numberOfIDF = ByteHelper.ReadInt16LE(binaryReader);

            int width = -1;
            int height = -1;
            for (int i = 0; i < numberOfIDF; i++)
            {
                short field = ByteHelper.ReadInt16LE(binaryReader);

                switch (field)
                {
                    // https://www.awaresystems.be/imaging/tiff/tifftags/baseline.html
                    default:
                        binaryReader.ReadBytes(10);
                        break;
                    case 256: // image width
                        binaryReader.ReadBytes(6);
                        width = ByteHelper.ReadInt32LE(binaryReader);
                        break;
                    case 257: // image length
                        binaryReader.ReadBytes(6);
                        height = ByteHelper.ReadInt32LE(binaryReader);
                        break;
                }
                if (width != -1 && height != -1)
                    return new Size(width, height);
            }
            return Size.Empty;
        }

        private static Size DecodeTiffBE(BinaryReader binaryReader)
        {
            int idfStart = ByteHelper.ReadInt32BE(binaryReader);

            binaryReader.BaseStream.Seek(idfStart, SeekOrigin.Begin);

            int numberOfIDF = ByteHelper.ReadInt16BE(binaryReader);

            int width = -1;
            int height = -1;
            for (int i = 0; i < numberOfIDF; i++)
            {
                short field = ByteHelper.ReadInt16BE(binaryReader);

                switch (field)
                {
                    // https://www.awaresystems.be/imaging/tiff/tifftags/baseline.html
                    default:
                        binaryReader.ReadBytes(10);
                        break;
                    case 256: // image width
                        binaryReader.ReadBytes(6);
                        width = ByteHelper.ReadInt32BE(binaryReader);
                        break;
                    case 257: // image length
                        binaryReader.ReadBytes(6);
                        height = ByteHelper.ReadInt32BE(binaryReader);
                        break;
                }
                if (width != -1 && height != -1)
                    return new Size(width, height);
            }
            return Size.Empty;
        }

        private static Size DecodeBitmap(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(16);
            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            return new Size(width, height);
        }

        private static Size DecodeGif(BinaryReader binaryReader)
        {
            int width = binaryReader.ReadInt16();
            int height = binaryReader.ReadInt16();
            return new Size(width, height);
        }

        private static Size DecodePng(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(8);
            int width = ByteHelper.ReadInt32BE(binaryReader);
            int height = ByteHelper.ReadInt32BE(binaryReader);
            return new Size(width, height);
        }

        private static Size DecodeJfif(BinaryReader binaryReader)
        {
            while (binaryReader.ReadByte() == 0xff)
            {
                byte marker = binaryReader.ReadByte();
                short chunkLength = ByteHelper.ReadInt16BE(binaryReader);
                if (marker == 0xc0 || marker == 0xc2) // c2: progressive
                {
                    binaryReader.ReadByte();
                    int height = ByteHelper.ReadInt16BE(binaryReader);
                    int width = ByteHelper.ReadInt16BE(binaryReader);
                    return new Size(width, height);
                }

                if (chunkLength < 0)
                {
                    ushort uchunkLength = (ushort)chunkLength;
                    binaryReader.ReadBytes(uchunkLength - 2);
                }
                else
                {
                    binaryReader.ReadBytes(chunkLength - 2);
                }
            }

            return Size.Empty;
        }

        private static Size DecodeWebP(BinaryReader binaryReader)
        {
            // 'RIFF' already read   
            binaryReader.ReadBytes(4);

            if (ByteHelper.ReadInt32LE(binaryReader) != 1346520407)// 1346520407 : 'WEBP'
                return Size.Empty;

            switch (ByteHelper.ReadInt32LE(binaryReader))
            {
                case 540561494: // 'VP8 ' : lossy
                                // skip stuff we don't need
                    binaryReader.ReadBytes(7);

                    if (ByteHelper.ReadInt24LE(binaryReader) != 2752925) // invalid webp file
                        return Size.Empty;

                    return new Size(ByteHelper.ReadInt16LE(binaryReader), ByteHelper.ReadInt16LE(binaryReader));

                case 1278758998:// 'VP8L' : lossless
                                // skip stuff we don't need
                    binaryReader.ReadBytes(4);

                    if (binaryReader.ReadByte() != 47)// 0x2f : 47 1 byte signature
                        return Size.Empty;

                    byte[] b = binaryReader.ReadBytes(4);

                    return new Size(
                        1 + (((b[1] & 0x3F) << 8) | b[0]),
                        1 + ((b[3] << 10) | (b[2] << 2) | ((b[1] & 0xC0) >> 6)));
                // if something breaks put in the '& 0xF' but & oxf should do nothing in theory
                // because inclusive & with 1111 will leave the binary untouched
                //  1 + (((wh[3] & 0xF) << 10) | (wh[2] << 2) | ((wh[1] & 0xC0) >> 6))

                case 1480085590:// 'VP8X' : extended
                                // skip stuff we don't need
                    binaryReader.ReadBytes(8);
                    return new Size(1 + ByteHelper.ReadInt24LE(binaryReader), 1 + ByteHelper.ReadInt24LE(binaryReader));
            }

            return Size.Empty;
        }

        #endregion


    }
}
