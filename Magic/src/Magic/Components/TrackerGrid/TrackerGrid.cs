using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GBA;

namespace Magic.Components
{
    public class TrackerGrid : DataGridView
    {
        List<ProgressBar> Volumes;

        public TrackerGrid() : base()
        {
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.RowHeadersWidth = 30;
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ColumnHeaderMouseClick += this.ColumnHeaderMouseClicked;
            ColumnWidthChanged += this.PlaceVolumeBars;
            Scroll += this.PlaceVolumeBars;
            
            CellPainting += this.PaintCells;

            this.Volumes = new List<ProgressBar>();
            this.Font = new System.Drawing.Font("Consolas", 7.5F);
        }

        public void Load(List<Track> tracks)
        {
            foreach (ProgressBar bar in this.Volumes)
            {
                bar.Dispose();
            }
            this.Volumes.Clear();
            this.Columns.Clear();
            this.Controls.Clear();
            try
            {
                ArrayFile notes = new ArrayFile("Music Notes.txt");
                Int32 width = this.RowHeadersWidth;
                for (Int32 i = 0; i < tracks.Count; i++)
                {
                    this.Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Track " + i + "\n\n" + "Notes |Vol|Ins|Pan| Effects",
                        MinimumWidth = 200,
                        SortMode = DataGridViewColumnSortMode.Programmatic
                    });
                    width += this.Columns[i].HeaderCell.Size.Width;
                    ProgressBar volume = new ProgressBar();
                    this.Volumes.Add(volume);
                    this.Controls.Add(volume);

                    String[][] track = tracks[i].GetTrackerString(notes);
                    const Int32 height = 14;
                    for (Int32 j = 0; j < track.Length; j++)
                    {
                        if (this.RowCount <= j) this.Rows.Add(new DataGridViewRow() { Height = height });
                        this.Rows[j].HeaderCell.Value = Util.IntToHex((UInt32)j);
                        this.Rows[j].Cells[i].Value = track[j];
                        this.Rows[j].Height = Math.Max(
                            (track[j][0] == null) ? height : track[j][0].Split('\n').Length * height,
                            (track[j][4] == null) ? height : track[j][4].Split('\n').Length * height);
                    }
                }
                this.PlaceVolumeBars(this, null);
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while loading tracker datagrid.", ex);
            }
        }
        
        void PaintCells(Object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                using (SolidBrush
                    backbrush = new SolidBrush(e.CellStyle.BackColor),
                    selection = new SolidBrush(SystemColors.Highlight))
                {
                    if (this.SelectedRows.Contains(this.Rows[e.RowIndex]))
                    {
                        e.Graphics.FillRectangle(selection, e.CellBounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(backbrush, e.CellBounds);
                    }
                }
                using (Pen gridLines = new Pen(this.GridColor))
                {
                    // Draw the grid lines (only the right and bottom lines)
                    e.Graphics.DrawLine(gridLines,
                        e.CellBounds.Left,      e.CellBounds.Bottom - 1,
                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    e.Graphics.DrawLine(gridLines,
                        e.CellBounds.Right - 1, e.CellBounds.Top,
                        e.CellBounds.Right - 1, e.CellBounds.Bottom);

                    // Draw the text content of the cell, ignoring alignment.
                    if (e.Value != null)
                    {
                        using (Brush brush = new SolidBrush(e.CellStyle.ForeColor))
                        {
                            if (e.Value is String)
                            {
                                e.Graphics.DrawString(
                                    (String)e.Value,
                                    e.CellStyle.Font,
                                    Brushes.Black,
                                    e.CellBounds.X + 2,
                                    e.CellBounds.Y);
                            }
                            else
                            {
                                String[] columns = (String[])e.Value;
                                Int32 width = 0;
                                for (Int32 i = 0; i < 5; i++)
                                {
                                    if (columns[i] != null)
                                    {
                                        e.Graphics.DrawString(
                                            columns[i],
                                            e.CellStyle.Font,
                                            Brushes.Black,
                                            e.CellBounds.X + width + 2,
                                            e.CellBounds.Y);
                                    }
                                    switch (i)
                                    {
                                        case 0: width += 50; break;
                                        case 1: width += 20; break;
                                        case 2: width += 20; break;
                                        case 3: width += 20; break;
                                        case 4: width += 90; break;
                                    }
                                    e.Graphics.DrawLine(gridLines,
                                        e.CellBounds.Left + width, e.CellBounds.Top,
                                        e.CellBounds.Left + width, e.CellBounds.Bottom);
                                }
                            }
                        }
                    }
                    e.Handled = true;
                }
            }
        }

        void ColumnHeaderMouseClicked(Object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.Volumes[e.ColumnIndex].Visible)
            {
                this.Volumes[e.ColumnIndex].Visible = false;

                this.Columns[e.ColumnIndex].ReadOnly = true;
                this.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                this.Columns[e.ColumnIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGray;
            }
            else
            {
                this.Volumes[e.ColumnIndex].Visible = true;

                this.Columns[e.ColumnIndex].ReadOnly = false;
                this.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                this.Columns[e.ColumnIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }
        void PlaceVolumeBars(Object sender, EventArgs e)
        {
            Int32 width = this.RowHeadersWidth;
            Int32 scroll = this.HorizontalScrollingOffset;
            for (Int32 i = 0; i < this.Volumes.Count; i++)
            {
                this.Volumes[i].Location = new Point(width + 4 - scroll, 20);
                this.Volumes[i].Size = new Size(192, 10);
                width += this.Columns[i].HeaderCell.Size.Width;
            }
        }
    }
}
