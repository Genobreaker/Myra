/* Generated by Myra UI Editor at 11/16/2017 2:38:36 AM */

using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;
using Myra.Graphics2D.UI.File;

namespace Myra.Samples.AllWidgets
{
	public partial class AllWidgets : Grid
	{
		public AllWidgets()
		{
			BuildUI();

			_buttonOpenFile.Image = DefaultAssets.UISpritesheet["icon-star"];
			_buttonOpenFile.Click += (sender, args) =>
			{
				var fileDialog = new FileDialog(FileDialogMode.OpenFile);
				fileDialog.ShowModal(Desktop);

				fileDialog.Closed += (s, a) =>
				{
					if (!fileDialog.Result)
					{
						return;
					}

					_textOpenFile.Text = fileDialog.FilePath;
				};
			};

			_buttonSaveFile.Image = DefaultAssets.UISpritesheet["icon-star"];
			_buttonSaveFile.Click += (sender, args) =>
			{
				var fileDialog = new FileDialog(FileDialogMode.SaveFile);
				fileDialog.ShowModal(Desktop);

				fileDialog.Closed += (s, a) =>
				{
					if (!fileDialog.Result)
					{
						return;
					}

					_textSaveFile.Text = fileDialog.FilePath;
				};
			};

			_buttonChooseFolder.Image = DefaultAssets.UISpritesheet["icon-star"];
			_buttonChooseFolder.Click += (sender, args) =>
			{
				var fileDialog = new FileDialog(FileDialogMode.ChooseFolder);
				fileDialog.ShowModal(Desktop);

				fileDialog.Closed += (s, a) =>
				{
					if (!fileDialog.Result)
					{
						return;
					}

					_textChooseFolder.Text = fileDialog.FilePath;
				};
			};

			_buttonChooseColor.Click += (sender, args) =>
			{
				var debugWindow = new ColorPickerDialog();
				debugWindow.ShowModal(Desktop);

				debugWindow.Closed += (s, a) =>
				{
					if (!debugWindow.Result)
					{
						return;
					}

					_textButtonLabel.TextColor = debugWindow.Color;
				};
			};

			_imageButton.Image = DefaultAssets.UISpritesheet["icon-star-outline"];
			_imageButton.Click += (sender, args) =>
			{
				var debugWindow = new DebugOptionsDialog();
				debugWindow.ShowModal(Desktop);
			};

			_menuItemAbout.Selected += (sender, args) =>
			{
				var messageBox = Dialog.CreateMessageBox("AllWidgets", "Myra AllWidgets Sample " + MyraEnvironment.Version);
				messageBox.ShowModal(Desktop);
			};

			var tree = new Tree
			{
				HasRoot = false,
				GridColumn = 1,
				GridRow = 12,
				GridColumnSpan = 2
			};
			var node1 = tree.AddSubNode("node1");
			var node2 = node1.AddSubNode("node2");
			var node3 = node2.AddSubNode("node3");
			node3.AddSubNode("node4");
			node3.AddSubNode("node5");
			node3.AddSubNode("node7");
			node3.AddSubNode("node8");
			node3.AddSubNode("node9");
			node3.AddSubNode("node10");

			var node4 = node2.AddSubNode("node6");
			node4.AddSubNode("node11");
			node4.AddSubNode("node12");
			node4.AddSubNode("node13");
			node4.AddSubNode("node14");
			node4.AddSubNode("node15");
			node4.AddSubNode("node16");
			node4.AddSubNode("node17");
			node4.AddSubNode("node18");
			_gridRight.Widgets.Add(tree);
		}
	}
}