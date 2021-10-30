using System;
using NET01.Enums;

namespace NET01.Entities
{
    public class TextMaterial : TrainingMaterials
    {
        private const int TextLength = 10000;
        private string _text;
        public string Text
        {
            get => _text;
            protected init
            {
                if (value.Length > TextLength)
                {
                    throw new ArgumentException($"Text is too long!(Max length = {TextLength})");
                }

                _text = value;
            }
        }

        public TextMaterial(string text, string description)
        {
            ID = this.InitGuid();
            Description = description;
            Text = text;
            MaterialType = MaterialType.Text;
        }

        public override TextMaterial Clone()
        {
            return new TextMaterial(Text, Description)
            {
                ID = this.ID
            };
        }
    }
}