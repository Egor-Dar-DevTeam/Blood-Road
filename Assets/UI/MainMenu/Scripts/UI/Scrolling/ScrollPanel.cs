using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scrolling
{
   public class ScrollPanel : MonoBehaviour
   {
      [SerializeField] private Image image;
      [SerializeField] private TextMeshProUGUI progress;
      private LevelInfo _levelInfo;
      
      
      public void SetTMProName(TextMeshProUGUI locationName)
      {
         var locationInfo = _levelInfo.GetCopy();
         locationName.text = locationInfo.Name;
      }

      public void SetLocationInfo(LevelInfo levelInfo)
      {
         _levelInfo = levelInfo;
         SetInfo();
      }
      private void SetInfo()
      {
         var locationInfo = _levelInfo.GetCopy();
         image.sprite = locationInfo.Sprite;
         progress.text = locationInfo.Progress;
      }

   }
}
