using MirishitaMusicPlayer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class IdolOrderForm : Form
    {
        private static readonly int[] positionToIndexTable = new int[]
        {
            2, 1, 3, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };

        private static readonly int[] indexToPositionTable = new int[]
        {
            3, 1, 0, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };

        private readonly Idol[] singers;
        private readonly int maxVoiceCount;
        private readonly List<CheckBox> idolCheckBoxes = new();
        private CheckBox sourceCheckBox;

        public IdolOrderForm(Idol[] order, int voiceCount)
        {
            InitializeComponent();

            singers = order;
            maxVoiceCount = voiceCount;
        }

        public Idol[] ResultOrder { get; private set; }

        private void IdolOrderForm_Load(object sender, EventArgs e)
        {
            if (singers.Length > 5)
                eightIdolPanel.Visible = true;

            for (int i = 0; i < singers.Length; i++)
            {
                Idol idol = singers[i];
                CheckBox checkBox = new();
                checkBox.Appearance = Appearance.Button;
                checkBox.Anchor = AnchorStyles.None;
                checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                checkBox.BackgroundImage = Resources.ResourceManager.GetObject($"icon_{idol.IdolNameID}") as Bitmap;
                checkBox.BackgroundImage.Tag = idol.IdolNameID;
                //checkBox.Dock = DockStyle.Fill;
                checkBox.Width = 100;
                checkBox.Height = 100;
                checkBox.Tag = i;

                checkBox.CheckedChanged += CheckBox_CheckedChanged;

                idolCheckBoxes.Add(checkBox);
            }

            int idolPosition = 0;
            for (int i = 0; i < maxVoiceCount; i++)
            {
                CheckBox checkBox = idolCheckBoxes[i];

                int column = positionToIndexTable[(int)checkBox.Tag];
                //checkBox.Tag = column;

                if (idolPosition < 5)
                    fiveIdolPanel.Controls.Add(checkBox, column, 0);
                else if (idolPosition >= 5 && idolPosition < 13)
                    eightIdolPanel.Controls.Add(checkBox, column % 5, 0);

                idolPosition++;
            }

            for (int i = maxVoiceCount; i < singers.Length; i++)
            {
                flowLayoutPanel1.Controls.Add(idolCheckBoxes[i]);
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Checked)
            {
                if (sourceCheckBox == null)
                    sourceCheckBox = checkBox;
                else
                {
                    int sourceColumn = fiveIdolPanel.GetColumn(sourceCheckBox);
                    int targetColumn = fiveIdolPanel.GetColumn(checkBox);

                    fiveIdolPanel.SetColumn(sourceCheckBox, targetColumn);
                    fiveIdolPanel.SetColumn(checkBox, sourceColumn);

                    int temp = (int)sourceCheckBox.Tag;
                    sourceCheckBox.Tag = checkBox.Tag;
                    checkBox.Tag = temp;

                    if (sourceCheckBox.Parent != checkBox.Parent)
                    {
                        if (sourceCheckBox.Parent.GetType() == typeof(TableLayoutPanel))
                            StashSwap(sourceCheckBox, checkBox);
                        else
                            StashSwap(checkBox, sourceCheckBox);
                    }

                    foreach (var idolCheckBox in idolCheckBoxes)
                        idolCheckBox.Checked = false;

                    sourceCheckBox = null;
                }
            }
            else
            {
                sourceCheckBox = null;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            int orderLength = soloCheckBox.Checked ? 1 : maxVoiceCount;

            ResultOrder = new Idol[orderLength];

            if (orderLength > 1)
            {
                foreach (var checkBox in idolCheckBoxes)
                {
                    int index = (int)checkBox.Tag;

                    if (index < maxVoiceCount)
                        ResultOrder[index] = new Idol((string)checkBox.BackgroundImage.Tag);
                }
            }
            else
                ResultOrder[0] = new Idol((string)idolCheckBoxes.First(c => (int)c.Tag == 0).Image.Tag);

            Close();
        }

        private void StashSwap(Control toStash, Control toIdol)
        {
            TableLayoutPanel idolPanel = toStash.Parent as TableLayoutPanel;
            Panel stashPanel = toIdol.Parent as Panel;

            int stashIndex = stashPanel.Controls.IndexOf(toIdol);

            List<Control> newControlList = new();
            for (int i = 0; i < stashPanel.Controls.Count; i++)
            {
                if (i == stashIndex)
                    newControlList.Add(toStash);
                else
                    newControlList.Add(stashPanel.Controls[i]);
            }
            stashPanel.Controls.Clear();
            stashPanel.Controls.AddRange(newControlList.ToArray());

            idolPanel.Controls.Add(toIdol, positionToIndexTable[(int)toIdol.Tag], 0);
            idolPanel.Controls.Remove(toStash);
        }
    }
}
