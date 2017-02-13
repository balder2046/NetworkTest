using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using autodemo;

public class TextureHelper : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    static ImageMsg MsgFromTexture(Texture2D text)
    {
        int width = text.width;
        int height = text.height;   
        int channels = 4;
        Color32[] pixels = text.GetPixels32();
        byte[] buf = new byte[width * height * channels];
		// to bgr
		int index = 0;
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x,++index) {
				buf [4 * index] = pixels [index].r;
				buf [4 * index + 1] = pixels [index].g;
				buf [4 * index + 2] = pixels [index].b;
				buf [4 * index + 3] = pixels [index].a;
			}
		}
		ImageMsg msg = new ImageMsg ();
		msg.width = width;
		msg.height = height;
		msg.channels = channels;
		msg.image_data = buf;
		return msg;
    }
	static Texture2D TextureFromMsg(ImageMsg msg)
	{
		Texture2D tex = new Texture2D (msg.width, msg.height, TextureFormat.ARGB32, false);
		int width = msg.width;
		int height = msg.height;
		int index = 0;
		Color32[] pixels = new Color32[width * height];
		for (int y = 0; y < height; ++y) {
			for (int x = 0; x < width; ++x,++index) {
				byte r = msg.image_data[4 * index];
				byte g = msg.image_data [4 * index + 1];
				byte b = msg.image_data [4 * index + 2];
				byte a = 255;
				pixels [index] = new Color32 (r,g,b,a);
			}
		}
		tex.SetPixels32 (pixels);
		tex.Apply ();
		return tex;
	}
}
