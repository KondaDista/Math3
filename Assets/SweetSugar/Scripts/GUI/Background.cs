﻿// // ©2015 - 2023 Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using SweetSugar.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SweetSugar.Scripts.GUI
{
	/// <summary>
	/// Background selector. Select different level background for every 20 levels
	/// </summary>
	public class Background : MonoBehaviour
	{
		public Sprite[] pictures;

		// Use this for initialization
		void OnEnable ()
		{
			if (LevelManager.THIS != null)
			{
				var backgroundSpriteNum = (int) (PlayerPrefs.GetInt("OpenLevel") / 20f - 0.01f);
				if(pictures.Length > backgroundSpriteNum)
					GetComponent<Image>().sprite = pictures[backgroundSpriteNum];
			}


		}


	}
}