using Gsemac.Forms.Styles.Extensions;
using Gsemac.Forms.Styles.StyleSheets;
using System.Drawing;
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

        public override void InitializeControl(DataGridView dataGridView) {

            INode controlNode = new ControlNode(dataGridView);
            IRuleset ruleset = styleSheet.GetRuleset(controlNode);

            if (ruleset.BackgroundColor.HasValue())
                dataGridView.BackgroundColor = ruleset.BackgroundColor.Value;

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

                styleRenderer.PaintBackground(e.Graphics, e.CellBounds, ruleset);

                e.PaintContent(e.ClipBounds);

                styleRenderer.PaintBorder(e.Graphics, e.CellBounds, ruleset);

                e.Handled = true;

            }

        }

        public override void PaintControl(DataGridView control, ControlPaintArgs args) {

            // Draw the background/border of the control.

            IRuleset ruleset = args.StyleSheet.GetRuleset(control);
            Rectangle borderRect = RenderUtilities.GetOuterBorderRectangle(control, ruleset);

            args.PaintBackground(borderRect);
            args.PaintBorder(borderRect);

        }

        // Private members

        private readonly IStyleSheet styleSheet;
        private readonly IStyleRenderer styleRenderer;

    }

}