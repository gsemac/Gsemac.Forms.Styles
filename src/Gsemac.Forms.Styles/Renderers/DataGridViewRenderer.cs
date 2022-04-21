using Gsemac.Forms.Styles.Dom;
using Gsemac.Forms.Styles.Renderers.Extensions;
using Gsemac.Forms.Styles.Renderers2;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Styles.StyleSheets.Extensions;
using Gsemac.Forms.Styles.StyleSheets.Properties;
using Gsemac.Forms.Styles.StyleSheets.Rulesets;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class DataGridViewRenderer :
        ControlRendererBase<DataGridView>,
        IDataGridViewRenderer {

        // Public members

        public bool RespectNonDefaultCellColors { get; set; } = true;

        public DataGridViewRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        public void RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) { }
        public void RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) { }
        public void CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {

            if (sender is DataGridView dataGridView) {

                bool isColumnHeader = DataGridViewUtilities.IsColumnHeaderIndex(e.RowIndex);
                bool isRowHeader = DataGridViewUtilities.IsRowHeaderIndex(e.ColumnIndex);
                bool isHeader = isColumnHeader || isRowHeader;
                bool isSelected = e.State.HasFlag(DataGridViewElementStates.Selected);

                UserNode cellNode = new UserNode(e.CellBounds, dataGridView.PointToClient(Cursor.Position));

                cellNode.AddClass("Cell");
                cellNode.SetParent(new ControlNode(sender as DataGridView));

                if (isColumnHeader)
                    cellNode.AddClass("ColumnHeader");
                else if (isRowHeader)
                    cellNode.AddClass("RowHeader");

                if (isHeader)
                    cellNode.AddClass("Header");

                if (e.RowIndex % 2 == 0)
                    cellNode.AddClass("Even");
                else
                    cellNode.AddClass("Odd");

                if (isSelected)
                    cellNode.AddState(NodeStates.Checked);

                IRuleset ruleset = styleSheet.GetRuleset(cellNode);

                if (RespectNonDefaultCellColors)
                    ruleset = SetNonDefaultCellColors(ruleset, dataGridView, e);

                if (ruleset.Color.HasValue()) {

                    if (isSelected)
                        e.CellStyle.SelectionForeColor = ruleset.Color.Value;
                    else
                        e.CellStyle.ForeColor = ruleset.Color.Value;

                }

                if (ruleset.BackgroundColor.HasValue()) {

                    if (isSelected)
                        e.CellStyle.SelectionBackColor = ruleset.BackgroundColor.Value;
                    else
                        e.CellStyle.BackColor = ruleset.BackgroundColor.Value;

                }

                if (ruleset.Opacity.HasValue() && ruleset.Opacity.Value < 1.0f) {

                    // Draw the part of the DataGridView background behind the cell.

                    styleRenderer.PaintParentBackground(e.Graphics, e.CellBounds, dataGridView.ClientRectangle, styleSheet.GetRuleset(dataGridView));

                }

                styleRenderer.PaintBackground(e.Graphics, e.CellBounds, ruleset);

                e.PaintContent(e.ClipBounds);

                styleRenderer.PaintBorder(e.Graphics, e.CellBounds, ruleset);

                e.Handled = true;

            }

        }
        public void Paint(object sender, PaintEventArgs e) {

            if (sender is DataGridView dataGridView) {

                Region clippingRegion = new Region(dataGridView.ClientRectangle);

                clippingRegion.Exclude(DataGridViewUtilities.GetVisibleRowsBounds(dataGridView, true));

                e.Graphics.SetClip(clippingRegion, CombineMode.Replace);

                styleRenderer.PaintBackground(e.Graphics, dataGridView.ClientRectangle, styleSheet.GetRuleset(dataGridView));

            }

        }

        public override void PaintControl(DataGridView control, ControlPaintArgs args) {

            // Draw the background/border of the control.

            IRuleset ruleset = args.StyleSheet.GetRuleset(control);
            Rectangle borderRect = Renderers2.RenderUtilities.GetOuterBorderRectangle(control.ClientRectangle, ruleset);

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor)
                control.BackgroundColor = ruleset.BackgroundColor.Value;

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

        private IRuleset SetNonDefaultCellColors(IRuleset ruleset, DataGridView dataGridView, DataGridViewCellPaintingEventArgs e) {

            IRuleset result = new Ruleset(ruleset);
            DataGridViewCellStyle cellStyle = DataGridViewUtilities.GetDefaultCellStyle(dataGridView, e.RowIndex, e.ColumnIndex);

            bool isSelected = e.State.HasFlag(DataGridViewElementStates.Selected);

            if (!isSelected && e.CellStyle.ForeColor != cellStyle.ForeColor)
                result.AddProperty(Property.Create(PropertyType.Color, e.CellStyle.ForeColor));
            else if (isSelected && e.CellStyle.SelectionForeColor != cellStyle.SelectionForeColor)
                result.AddProperty(Property.Create(PropertyType.Color, e.CellStyle.SelectionForeColor));

            if (!isSelected && e.CellStyle.BackColor != cellStyle.BackColor)
                result.AddProperty(Property.Create(PropertyType.BackgroundColor, e.CellStyle.BackColor));
            else if (isSelected && e.CellStyle.SelectionBackColor != cellStyle.SelectionBackColor)
                result.AddProperty(Property.Create(PropertyType.BackgroundColor, e.CellStyle.SelectionBackColor));

            return result;

        }

    }

}