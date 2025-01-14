﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace MailFrameworkMod.Api
{
    public class MailFrameworkModApi : IMailFrameworkModApi
    {
        public void RegisterContentPack(IContentPack contentPack)
        {
            DataLoader.RegisterContentPack(contentPack);
        }

        public void RegisterLetter(ILetter iLetter, Func<ILetter, bool> condition, Action<ILetter> callback = null, Func<ILetter, List<Item>> dynamicItems = null)
        {
            Letter letter;
            if (iLetter.Recipe != null)
            {
                letter = new Letter(iLetter.Id, iLetter.Text, iLetter.Recipe, (l) => condition.Invoke(new ApiLetter(l)));
            }
            else if (iLetter.Items != null)
            {
                letter = new Letter(iLetter.Id, iLetter.Text, iLetter.Items, (l) => condition.Invoke(new ApiLetter(l)));
            }
            else
            {
                letter = new Letter(iLetter.Id, iLetter.Text, (l) => condition.Invoke(new ApiLetter(l)));
            }

            letter.GroupId = iLetter.GroupId;
            letter.Title = iLetter.Title;
            if (callback != null)
            {
                letter.Callback = (l) => callback.Invoke(new ApiLetter(l));
            }
            if (dynamicItems != null)
            {
                letter.DynamicItems = (l) => dynamicItems.Invoke(new ApiLetter(l));
            }
            letter.WhichBG = iLetter.WhichBG;
            letter.LetterTexture = iLetter.LetterTexture;
            letter.TextColor = iLetter.TextColor;
            letter.UpperRightCloseButtonTexture = iLetter.UpperRightCloseButtonTexture;
            letter.AutoOpen = iLetter.AutoOpen;
            letter.I18N = iLetter.I18N;

            MailDao.SaveLetter(letter);
        }

        public ILetter GetLetter(string id)
        {
            return new ApiLetter(MailDao.FindLetter(id));
        }

        public string GetMailDataString(string id)
        {
            return MailDao.FindLetter(id)?.ToMailDataString();
        }
    }
}
