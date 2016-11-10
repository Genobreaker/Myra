﻿using System;
using Myra.Graphics2D.Text;
using Myra.Resources;
using Newtonsoft.Json.Linq;

namespace Myra.Graphics2D.UI.Styles
{
	public class Stylesheet
	{
		public TextBlockStyle TextBlockStyle { get; private set; }
		public TextFieldStyle TextFieldStyle { get; private set; }
		public ButtonStyle ButtonStyle { get; private set; }
		public ButtonStyle CheckBoxStyle { get; private set; }
		public ComboBoxStyle ComboBoxStyle { get; private set; }
		public TreeStyle TreeStyle { get; private set; }
		public SplitPaneStyle HorizontalSplitPaneStyle { get; private set; }
		public SplitPaneStyle VerticalSplitPaneStyle { get; private set; }
		public ScrollAreaStyle ScrollAreaStyle { get; private set; }
		public MenuStyle HorizontalMenuStyle { get; private set; }
		public MenuStyle VerticalMenuStyle { get; private set; }
		public WindowStyle WindowStyle { get; private set; }

		public Stylesheet()
		{
			TextBlockStyle = new TextBlockStyle();
			TextFieldStyle = new TextFieldStyle();
			ButtonStyle = new ButtonStyle();
			CheckBoxStyle = new ButtonStyle();
			ComboBoxStyle = new ComboBoxStyle();
			TreeStyle = new TreeStyle();
			HorizontalSplitPaneStyle = new SplitPaneStyle();
			VerticalSplitPaneStyle = new SplitPaneStyle();
			ScrollAreaStyle = new ScrollAreaStyle();
			HorizontalMenuStyle = new MenuStyle();
			VerticalMenuStyle = new MenuStyle();
			WindowStyle = new WindowStyle();
		}

		public static Stylesheet CreateFromSource(string source,
			Func<string, BitmapFont> fontGetter,
			Func<string, Drawable> drawableGetter)
		{
			var root = JObject.Parse(source);
			var loader = new StylesheetLoader(root, fontGetter, drawableGetter);

			return loader.CreateWidgetStyleFromSource();
		}
	}
}