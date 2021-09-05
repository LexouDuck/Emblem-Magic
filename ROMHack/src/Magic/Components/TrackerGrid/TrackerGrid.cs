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
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            RowHeadersWidth = 30;
            RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ColumnHeaderMouseClick += ColumnHeaderMouseClicked;
            ColumnWidthChanged += PlaceVolumeBars;
            Scroll += PlaceVolumeBars;
            
            CellPainting += PaintCells;

            Volumes = new List<ProgressBar>();
            Font = new System.Drawing.Font("Consolas", 7.5F);
        }

        public void Load(List<Track> tracks)
        {
            foreach (ProgressBar bar in Volumes)
            {
                bar.Dispose();
            }
            Volumes.Clear();
            Columns.Clear();
            Controls.Clear();
            try
            {
                ArrayFile notes = new ArrayFile("Music Notes.txt");
                int width = RowHeadersWidth;
                for (int i = 0; i < tracks.Count; i++)
                {
                    Columns.Add(new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Track " + i + "\n\n" + "Notes |Vol|Ins|Pan| Effects",
                        MinimumWidth = 200,
                        SortMode = DataGridViewColumnSortMode.Programmatic
                    });
                    width += Columns[i].HeaderCell.Size.Width;
                    ProgressBar volume = new ProgressBar();
                    Volumes.Add(volume);
                    Controls.Add(volume);

                    string[][] track = tracks[i].GetTrackerString(notes);
                    const int height = 14;
                    for (int j = 0; j < track.Length; j++)
                    {
                        if (RowCount <= j) Rows.Add(new DataGridViewRow() { Height = height });
                        Rows[j].HeaderCell.Value = Util.IntToHex((uint)j);
                        Rows[j].Cells[i].Value = track[j];
                        Rows[j].Height = Math.Max(
                            (track[j][0] == null) ? height : track[j][0].Split('\n').Length * height,
                            (track[j][4] == null) ? height : track[j][4].Split('\n').Length * height);
                    }
                }
                PlaceVolumeBars(this, null);
            }
            catch (Exception ex)
            {
                UI.ShowError("Error while loading tracker datagrid.", ex);
            }
        }
        
        void PaintCells(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                using (SolidBrush
                    backbrush = new SolidBrush(e.CellStyle.BackColor),
                    selection = new SolidBrush(SystemColors.Highlight))
                {
                    if (SelectedRows.Contains(Rows[e.RowIndex]))
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
                            if (e.Value is string)
                            {
                                e.Graphics.DrawString(
                                    (string)e.Value,
                                    e.CellStyle.Font,
                                    Brushes.Black,
                                    e.CellBounds.X + 2,
                                    e.CellBounds.Y);
                            }
                            else
                            {
                                string[] columns = (string[])e.Value;
                                int width = 0;
                                for (int i = 0; i < 5; i++)
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

        void ColumnHeaderMouseClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Volumes[e.ColumnIndex].Visible)
            {
                Volumes[e.ColumnIndex].Visible = false;

                Columns[e.ColumnIndex].ReadOnly = true;
                Columns[e.ColumnIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                Columns[e.ColumnIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGray;
            }
            else
            {
                Volumes[e.ColumnIndex].Visible = true;

                Columns[e.ColumnIndex].ReadOnly = false;
                Columns[e.ColumnIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                Columns[e.ColumnIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }
        void PlaceVolumeBars(object sender, EventArgs e)
        {
            int width = RowHeadersWidth;
            int scroll = HorizontalScrollingOffset;
            for (int i = 0; i < Volumes.Count; i++)
            {
                Volumes[i].Location = new Point(width + 4 - scroll, 20);
                Volumes[i].Size = new Size(192, 10);
                width += Columns[i].HeaderCell.Size.Width;
            }
        }
    }
}
