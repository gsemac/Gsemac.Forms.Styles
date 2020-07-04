using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using Gsemac.Forms.Utilities;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public class DataGridViewRenderer :
        ControlRendererBase<DataGridView>,
        IDataGridViewRenderer {

        // Public members

        public DataGridViewRenderer(IStyleSheet styleSheet, IStyleRenderer styleRenderer) {

            this.styleSheet = styleSheet;
            this.styleRenderer = styleRenderer;

        }

        public void RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e) { }
        public void RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e) { }
        public void CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {

            if (sender is DataGridView dataGridView) {

                bool isColumnHeader = e.RowIndex < 0;
                bool isRowHeader = e.ColumnIndex < 0;
                bool isSelected = e.State.HasFlag(DataGridViewElementStates.Selected);

                UserNode cellNode = new UserNode(e.CellBounds, dataGridView.PointToClient(Cursor.Position));

                cellNode.AddClass("Cell");
                cellNode.SetParent(new ControlNode(sender as DataGridView));

                if (isColumnHeader)
                    cellNode.AddClass("ColumnHeader");
                else if (isRowHeader)
                    cellNode.AddClass("RowHeader");

                if (isColumnHeader || isRowHeader)
                    cellNode.AddClass("Header");

                if (e.RowIndex % 2 == 0)
                    cellNode.AddClass("Even");
                else
                    cellNode.AddClass("Odd");

                if (isSelected)
                    cellNode.AddState(NodeStates.Checked);

                IRuleset ruleset = styleSheet.GetRuleset(cellNode);

                if (ruleset.Color.HasValue()) {

                    if (isColumnHeader) {

                        if (!isSelected && e.CellStyle.ForeColor == dataGridView.ColumnHeadersDefaultCellStyle.ForeColor)
                            e.CellStyle.ForeColor = ruleset.Color.Value;
                        else if (isSelected && e.CellStyle.SelectionForeColor == dataGridView.ColumnHeadersDefaultCellStyle.SelectionForeColor)
                            e.CellStyle.SelectionForeColor = ruleset.Color.Value;

                    }
                    else if (isRowHeader) {

                        if (!isSelected && e.CellStyle.ForeColor == dataGridView.RowHeadersDefaultCellStyle.ForeColor)
                            e.CellStyle.ForeColor = ruleset.Color.Value;
                        else if (isSelected && e.CellStyle.SelectionForeColor == dataGridView.RowHeadersDefaultCellStyle.SelectionForeColor)
                            e.CellStyle.SelectionForeColor = ruleset.Color.Value;

                    }
                    else if (!isSelected && e.CellStyle.ForeColor == dataGridView.DefaultCellStyle.ForeColor) {

                        e.CellStyle.ForeColor = ruleset.Color.Value;

                    }
                    else if (isSelected && e.CellStyle.SelectionForeColor == dataGridView.DefaultCellStyle.SelectionForeColor) {

                        e.CellStyle.SelectionForeColor = ruleset.Color.Value;

                    }

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
            Rectangle borderRect = RenderUtilities.GetOuterBorderRectangle(control, ruleset);

            if (ruleset.BackgroundColor.HasValue() && ruleset.BackgroundColor.Value != control.BackColor)
                control.BackgroundColor = ruleset.BackgroundColor.Value;

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

    }

}