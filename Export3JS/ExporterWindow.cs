﻿using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;

namespace Export3JS {

    public class ExporterWindow : EditorWindow {

        private ExporterOptions options;
        private bool exportAll;
        private bool selectTags;

        [MenuItem("ThreeJS/Export %#e")]
        static void Init() {
            ExporterWindow window = (ExporterWindow)GetWindow(typeof(ExporterWindow));
            window.titleContent = new GUIContent("ThreeJS");
            window.Show();
        }

        void OnEnable() {
            options = new ExporterOptions();
            options.dir = string.Empty;
            options.exportLights = true;
            options.exportMeshes = true;
            options.exportCameras = true;
            options.exportDisabled = true;
            options.castShadows = false;
            options.tags = new string[0];
            selectTags = false;
            options.forceDoubleSidedMaterials = false;
            options.appendPNGExtensionToTextureURLs = false;
            options.minifyJSON = true;
        }

        void OnGUI() {

            // Toggle options
            exportAll = options.exportLights && options.exportMeshes && options.exportCameras && options.exportDisabled;

            GUILayout.BeginVertical();
            GUILayout.Label("Export options", EditorStyles.boldLabel);
            GUILayout.Label("Choose what to export:", EditorStyles.boldLabel);
            if (EditorGUILayout.Toggle("All", exportAll)) {
                options.exportCameras = true;
                options.exportLights = true;
                options.exportMeshes = true;
                options.exportDisabled = true;
            }
            options.exportMeshes = EditorGUILayout.Toggle("Meshes", options.exportMeshes);
            options.exportCameras = EditorGUILayout.Toggle("Cameras", options.exportCameras);
            options.exportLights = EditorGUILayout.Toggle("Lights", options.exportLights);
            options.exportDisabled = EditorGUILayout.Toggle("Disabled GameObjects", options.exportDisabled);
            selectTags = EditorGUILayout.Foldout(selectTags, new GUIContent("Tags", "Create list of UUIDs, matching selected tags"));
            if (selectTags) {
                int count = options.tags.Length;
                count = EditorGUILayout.IntField("Size", count);
                if (count != options.tags.Length) options.tags = new string[count];
                for (int i = 0; i < options.tags.Length; i++) {
                    options.tags[i] = EditorGUILayout.TagField("Tag " + i, options.tags[i]);
                }
            }
            EditorGUILayout.Space();
            GUILayout.Label("Shadows", EditorStyles.boldLabel);
            options.castShadows = EditorGUILayout.Toggle("Cast shadows", options.castShadows);
            EditorGUILayout.Space();
            GUILayout.Label("Other settings", EditorStyles.boldLabel);
            options.forceDoubleSidedMaterials = EditorGUILayout.Toggle(new GUIContent("Force double sided materials",
                "Use this if you are stuck with using a model that has faces with wrong normals"),
                options.forceDoubleSidedMaterials);
            options.appendPNGExtensionToTextureURLs = EditorGUILayout.Toggle(new GUIContent("Append .png to" +
                " Texture URL's", "In the output .json file, append .png to every texture URL. This option is to be" +
                " used together with an external image converter such as imagemagick to manually convert the textures" +
                " to png format. Note that the original file extension is not removed from the URL."),
                options.appendPNGExtensionToTextureURLs);
            options.minifyJSON = EditorGUILayout.Toggle("Minify JSON", options.minifyJSON);
            EditorGUILayout.Space();
            GUILayout.Label("Specify output location:", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            options.dir = GUILayout.TextField(options.dir);
            if (GUILayout.Button("...", GUILayout.ExpandWidth(false))) {
                string dir = EditorUtility.OpenFolderPanel("Choose destination folder", "", "");
                options.dir = dir + "/";
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Export", GUILayout.ExpandWidth(false))) {
                Exporter exporter = new Exporter(options);
                exporter.Export();
            }
            GUILayout.EndVertical();
        }

        public static void ReportProgress(float value, string message = "") {
            EditorUtility.DisplayProgressBar("ThreeJS", message, value);
        }

        public static void ClearProgress() {
            EditorUtility.ClearProgressBar();
        }
    }
}

#endif
