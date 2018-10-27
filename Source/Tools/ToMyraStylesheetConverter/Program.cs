using Microsoft.Xna.Framework;
using MonoGdx.Utils;
using Myra.Graphics2D;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Myra.Tools.ToMyraStylesheetConverter
{
	class Program
	{
		private class DrawableInfo
		{
			public string Name { get; set; }
			public Color Color { get; set; }
		}

		private class StyleInfo
		{
			public string Name { get; set; }
			public Dictionary<string, string> PropertyConversion { get; set; }
			public bool HorizontalAndVertical { get; set; }
		}

		private static readonly Dictionary<string, StyleInfo> _styles = new Dictionary<string, StyleInfo>
		{
			{
				"com.badlogic.gdx.scenes.scene2d.ui.CheckBox$CheckBoxStyle",
				new StyleInfo
				{
					Name = "checkBox",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "checkboxOn", "image/imagePressed" },
						{ "checkboxOff", "image/image" },
						{ "font", "label/font" },
						{ "fontColor", "label/textColor" },
						{ "disabledFontColor", "label/disabledTextColor" },
					}
				}
			},
			{
				"com.kotcrab.vis.ui.widget.VisCheckBox$VisCheckBoxStyle",
				new StyleInfo
				{
					Name = "checkBox",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "checkBackground", "background" },
						{ "checkBackgroundOver", "overBackground" },
						{ "tick", "image/imagePressed" },
						{ "font", "label/font" },
						{ "fontColor", "label/textColor" },
						{ "disabledFontColor", "label/disabledTextColor" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.ImageButton$ImageButtonStyle|com.kotcrab.vis.ui.widget.VisImageButton$VisImageButtonStyle",
				new StyleInfo
				{
					Name = "imageButton",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "up", "background" },
						{ "down", "pressedBackground" },
						{ "over", "overBackground" },
						{ "imageUp", "image/image" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.ImageTextButton$ImageTextButtonStyle|com.kotcrab.vis.ui.widget.VisImageTextButton$VisImageTextButtonStyle",
				new StyleInfo
				{
					Name = "button",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "up", "background" },
						{ "down", "pressedBackground" },
						{ "font", "label/font" },
						{ "fontColor", "label/textColor" },
						{ "disabledFontColor", "label/disabledTextColor" },
						{ "downFontColor", "label/downTextColor" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.Label$LabelStyle",
				new StyleInfo
				{
					Name = "textBlock",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "up", "background" },
						{ "down", "pressedBackground" },
						{ "font", "font" },
						{ "fontColor", "textColor" },
						{ "disabledFontColor", "disabledTextColor" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.List$ListStyle",
				new StyleInfo
				{
					Name = "listBox",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "font", "listBoxItem/label/font" },
						{ "fontColor", "listBoxItem/label/textColor" },
						{ "disabledFontColor", "listBoxItem/label/disabledTextColor" },
						{ "selection", "listBoxItem/pressedBackground" }
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.ProgressBar$ProgressBarStyle",
				new StyleInfo
				{
					Name = "listBox",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "knobBefore", "filled" }
					},
					HorizontalAndVertical = true
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.ScrollPane$ScrollPaneStyle",
				new StyleInfo
				{
					Name = "scrollArea",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "hScroll", "horizontalScroll" },
						{ "hScrollKnob", "horizontalScrollKnob" },
						{ "vScroll", "verticalScroll" },
						{ "vScrollKnob", "verticalScrollKnob" }
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.SelectBox$SelectBoxStyle",
				new StyleInfo
				{
					Name = "comboBox",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "font", "label/font" },
						{ "fontColor", "label/textColor" },
						{ "disabledFontColor", "label/disabledTextColor" },
						{ "listStyle/background", "itemsContainer/background" },
						{ "listStyle/font", "comboBoxItem/label/font" },
						{ "listStyle/fontColor", "comboBoxItem/label/textColor" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.Slider$SliderStyle",
				new StyleInfo
				{
					Name = "Slider",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "knob", "knob/image/image" },
						{ "knobOver", "knob/image/overImage" },
						{ "knobDown", "knob/image/pressedImage" },
						{ "disabledKnob", "knob/image/disabledImage" },
					},
					HorizontalAndVertical = true
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.SplitPane$SplitPaneStyle",
				new StyleInfo
				{
					Name = "SplitPane",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "handle", "handle/background" },
						{ "handleOver", "handle/overBackground" },
					},
					HorizontalAndVertical = true
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.TextButton$TextButtonStyle|com.kotcrab.vis.ui.widget.VisTextButton$VisTextButtonStyle",
				new StyleInfo
				{
					Name = "textButton",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "up", "background" },
						{ "down", "pressedBackground" },
						{ "over", "overBackground" },
						{ "font", "label/font" },
						{ "fontColor", "label/textColor" },
						{ "downFontColor", "label/downTextColor" },
						{ "disabledFontColor", "label/disabledTextColor" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.TextField$TextFieldStyle|com.kotcrab.vis.ui.widget.VisTextField$VisTextFieldStyle",
				new StyleInfo
				{
					Name = "textField",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "font", "font" },
						{ "fontColor", "textColor" },
						{ "disabledFontColor", "disabledTextColor" },
						{ "cursor", "cursor" },
						{ "selection", "selection" },
						{ "focusedFontColor", "focusedTextColor" },
						{ "focusedBackground", "focusedBackground" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.Tree$TreeStyle",
				new StyleInfo
				{
					Name = "tree",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "plus", "mark/image/image" },
						{ "minus", "mark/image/pressedImage" },
						{ "selection", "selectionBackground" },
						{ "over", "selectionHoverBackground" },
					}
				}
			},
			{
				"com.badlogic.gdx.scenes.scene2d.ui.Window$WindowStyle",
				new StyleInfo
				{
					Name = "window",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "titleFont", "title/font" },
						{ "titleFontColor", "title/textColor" },
						{ "over", "selectionHoverBackground" },
					}
				}
			},
			{
				"com.kotcrab.vis.ui.widget.Menu$MenuStyle",
				new StyleInfo
				{
					Name = "window",
					PropertyConversion = new Dictionary<string, string>
					{
						{ "background", "background" },
						{ "titleFont", "title/font" },
						{ "titleFontColor", "title/textColor" },
						{ "over", "selectionHoverBackground" },
					}
				}
			},
		};

		private static Color FromInputColor(object data)
		{
			var inputData = (Dictionary<string, object>)data;

			return new Color((float)inputData["r"],
				(float)inputData["g"],
				(float)inputData["b"],
				(float)inputData["a"]);
		}

		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("ToMyraStylesheetConverter converts LibGDX UI stylesheet to Myra UI stylesheet.");
				Console.WriteLine("Usage: ToMyraStylesheetConverter <input.json> <output.json>");

				return;
			}

			try
			{
				var inputFile = args[0];
				var outputFile = args[1];

				var inputText = File.ReadAllText(inputFile);

				Dictionary<string, object> input;
				using (var reader = new StringReader(inputText))
				{
					input = (Dictionary<string, object>)Json.Deserialize(reader);
				}

				object obj;
				Dictionary<string, Color> colors = new Dictionary<string, Color>();
				if (input.TryGetValue("com.badlogic.gdx.graphics.Color", out obj))
				{
					var inputColors = (Dictionary<string, object>)obj;
					foreach (var pair in inputColors)
					{
						colors[pair.Key] = FromInputColor(pair.Value);
					}
				}

				Dictionary<string, DrawableInfo> drawables = new Dictionary<string, DrawableInfo>();
				if (input.TryGetValue("com.badlogic.gdx.scenes.scene2d.ui.Skin$TintedDrawable", out obj))
				{
					var inputDrawables = (Dictionary<string, object>)obj;
					foreach (var pair in inputDrawables)
					{
						var inputData = (Dictionary<string, object>)pair.Value;

						var colorObject = inputData["color"];

						Color color;
						if (colorObject is string)
						{
							color = colors[(string)colorObject];
						} else
						{
							color = FromInputColor(colorObject);
						}

						drawables[pair.Key] = new DrawableInfo
						{
							Name = (string)inputData["name"],
							Color = color
						};
					}
				}

				var output = new JObject();

				foreach (var pair in _styles)
				{
					var parts = pair.Key.Split('|');
					if (input.TryGetValue(parts[parts.Length - 1], out obj))
					{
						var outputObj = new JObject();
						var inputVariants = (Dictionary<string, object>)obj;

						foreach (var pair2 in inputVariants)
						{
							if (pair2.Key == "default")
							{
								var inputProperties = (Dictionary<string, object>)pair2.Value;
								foreach (var pair3 in inputProperties)
								{
									string propName;
									if (pair.Value.PropertyConversion.TryGetValue(pair3.Key, out propName))
									{
										var parts2 = propName.Split('/').ToList();

										var outputObj2 = outputObj;
										while (parts2.Count > 1)
										{
											if (outputObj2[parts2[0]] == null)
											{
												outputObj2[parts2[0]] = new JObject();
											}

											outputObj2 = (JObject)outputObj2[parts2[0]];
											parts2.RemoveAt(0);
											propName = string.Join("/", parts2);
										}

										outputObj2[propName] = (string)pair3.Value;
									} else
									{
										Console.WriteLine("WARNING: Property '{0}' of item '{1}' wasn't processed.", pair3.Key, parts[parts.Length - 1]);
									}
								}
							}
						}

						output[pair.Value.Name] = outputObj;
					}
				}

				File.WriteAllText(outputFile, output.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
