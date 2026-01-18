using UnityEngine;
using System;
using System.IO;

namespace Monocle
{
    /// <summary>
    /// Unity replacement for XNA VirtualTexture.
    /// Uses Unity's Texture2D instead of XNA's Texture2D.
    /// </summary>
    public class VirtualTexture : VirtualAsset
    {
        private const int ByteArraySize = 0x8_0000;
        private const int ByteArrayCheckSize = ByteArraySize - 0x20;
        internal static readonly byte[] buffer = new byte[0x400_0000];
        internal static readonly byte[] bytes = new byte[ByteArraySize];
        public Texture2D Texture;
        private Color32 color;

        public string Path { get; private set; }

        public bool IsDisposed => Texture == null;

        internal VirtualTexture(string path)
        {
            Name = Path = path;
            Reload();
        }

        internal VirtualTexture(string name, int width, int height, Color32 color)
        {
            Name = name;
            Width = width;
            Height = height;
            this.color = color;
            Reload();
        }

        // Constructor for XNA Color compatibility
        internal VirtualTexture(string name, int width, int height, Microsoft.Xna.Framework.Color color)
        {
            Name = name;
            Width = width;
            Height = height;
            this.color = new Color32(color.R, color.G, color.B, color.A);
            Reload();
        }

        internal override void Unload()
        {
            if (Texture != null)
            {
                UnityEngine.Object.Destroy(Texture);
                Texture = null;
            }
        }

        internal override void Reload()
        {
            Unload();

            if (string.IsNullOrEmpty(Path))
            {
                // Set the texture to be filled with 'color'
                Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
                Color32[] data = new Color32[Width * Height];
                for (int i = 0; i < data.Length; i++)
                    data[i] = color;
                Texture.SetPixels32(data);
                Texture.Apply();
            }
            else
            {
                string extension = System.IO.Path.GetExtension(Path);
                string contentDirectory = Engine.ContentDirectory;
                
                if (extension == ".data")
                {
                    LoadDataFile(contentDirectory);
                }
                else if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                {
                    LoadImageFile(contentDirectory);
                }
                else
                {
                    // Try loading as generic image
                    LoadImageFile(contentDirectory);
                }
                
                Width = Texture.width;
                Height = Texture.height;
            }
        }

        private void LoadDataFile(string contentDirectory)
        {
            using FileStream fileStream = File.OpenRead(System.IO.Path.Combine(contentDirectory, Path));
            fileStream.Read(bytes, 0, ByteArraySize);

            int width = BitConverter.ToInt32(bytes, 0);
            int height = BitConverter.ToInt32(bytes, 4);
            bool flag = bytes[8] == 1;

            int bytesIndex = 9;
            int totalSize = width * height * 4;
            int bufferIndex = 0;
            
            while (bufferIndex < totalSize)
            {
                int blockSize = bytes[bytesIndex] * 4;
                if (flag)
                {
                    byte num2 = bytes[bytesIndex + 1];
                    if (num2 > 0)
                    {
                        buffer[bufferIndex] = bytes[bytesIndex + 4];
                        buffer[bufferIndex + 1] = bytes[bytesIndex + 3];
                        buffer[bufferIndex + 2] = bytes[bytesIndex + 2];
                        buffer[bufferIndex + 3] = num2;
                        bytesIndex += 5;
                    }
                    else
                    {
                        buffer[bufferIndex] = 0;
                        buffer[bufferIndex + 1] = 0;
                        buffer[bufferIndex + 2] = 0;
                        buffer[bufferIndex + 3] = 0;
                        bytesIndex += 2;
                    }
                }
                else
                {
                    buffer[bufferIndex] = bytes[bytesIndex + 3];
                    buffer[bufferIndex + 1] = bytes[bytesIndex + 2];
                    buffer[bufferIndex + 2] = bytes[bytesIndex + 1];
                    buffer[bufferIndex + 3] = byte.MaxValue;
                    bytesIndex += 4;
                }
                
                if (blockSize > 4)
                {
                    int index3 = bufferIndex + 4;
                    for (int index4 = bufferIndex + blockSize; index3 < index4; index3 += 4)
                    {
                        buffer[index3] = buffer[bufferIndex];
                        buffer[index3 + 1] = buffer[bufferIndex + 1];
                        buffer[index3 + 2] = buffer[bufferIndex + 2];
                        buffer[index3 + 3] = buffer[bufferIndex + 3];
                    }
                }
                
                bufferIndex += blockSize;
                
                if (bytesIndex > ByteArrayCheckSize)
                {
                    int offset = ByteArraySize - bytesIndex;
                    for (int index5 = 0; index5 < offset; ++index5)
                        bytes[index5] = bytes[bytesIndex + index5];
                    fileStream.Read(bytes, offset, ByteArraySize - offset);
                    bytesIndex = 0;
                }
            }
            
            Texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Texture.filterMode = FilterMode.Point;
            Texture.LoadRawTextureData(buffer);
            Texture.Apply();
        }

        private void LoadImageFile(string contentDirectory)
        {
            string fullPath = System.IO.Path.Combine(contentDirectory, Path);
            byte[] imageData = File.ReadAllBytes(fullPath);
            
            Texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            Texture.filterMode = FilterMode.Point;
            Texture.LoadImage(imageData);
            
            // Premultiply alpha (matching original behavior)
            Color[] pixels = Texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                float alpha = pixels[i].a;
                pixels[i].r *= alpha;
                pixels[i].g *= alpha;
                pixels[i].b *= alpha;
            }
            Texture.SetPixels(pixels);
            Texture.Apply();
        }

        public override void Dispose()
        {
            Unload();
            Texture = null;
            VirtualContent.Remove(this);
        }
    }
}
