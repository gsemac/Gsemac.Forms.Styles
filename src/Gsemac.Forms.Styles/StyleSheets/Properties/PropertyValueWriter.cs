using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Gsemac.Forms.Styles.StyleSheets.Properties {

    public class PropertyValueWriter :
        TextWriter,
        IPropertyValueWriter {

        // Public members

        public override Encoding Encoding { get; } = Encoding.UTF8;

        public static PropertyValueWriter Default => new PropertyValueWriter();

        public void Write(Color value) {

            Write(ColorTranslator.ToHtml(value).ToLowerInvariant());

        }
        public void Write(BorderStyle value) {

            switch (value) {

                case BorderStyle.Dotted:
                    Write("dotted");
                    break;

                case BorderStyle.Dashed:
                    Write("dashed");
                    break;

                case BorderStyle.Solid:
                    Write("solid");
                    break;

                case BorderStyle.Double:
                    Write("double");
                    break;

                case BorderStyle.Groove:
                    Write("groove");
                    break;

                case BorderStyle.Ridge:
                    Write("ridge");
                    break;

                case BorderStyle.Inset:
                    Write("inset");
                    break;

                case BorderStyle.Outset:
                    Write("outset");
                    break;

                case BorderStyle.None:
                    Write("none");
                    break;

                case BorderStyle.Hidden:
                    Write("hidden");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));

            }

        }

    }

}