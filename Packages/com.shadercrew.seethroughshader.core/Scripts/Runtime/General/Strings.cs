namespace ShaderCrew.SeeThroughShader
{
    public static class Strings
    {
        //public static readonly float stsVersion = 1.8f;
        public static readonly string SEE_THROUGH_SHADER_TITLE = "See-through Shader "; // + stsVersion;


        // ComponentMenu TITLES:
        public const string COMPONENTMENU_FOLDER = "See-through Shader/"; //FOLDER

        public const string COMPONENTMENU_SHADER_REPLACEMENT_FOLDER = COMPONENTMENU_FOLDER + "Shader Replacement and Sync/"; //FOLDER
        public const string COMPONENTMENU_GLOBAL_SHADER_REPLACEMENT = COMPONENTMENU_SHADER_REPLACEMENT_FOLDER + "STS-Global Shader Replacement";
        public const string COMPONENTMENU_GROUP_SHADER_REPLACEMENT = COMPONENTMENU_SHADER_REPLACEMENT_FOLDER +"STS-Group Shader Replacement";
        public const string COMPONENTMENU_SHADER_EXEMPTION = COMPONENTMENU_SHADER_REPLACEMENT_FOLDER + "STS-Shader Exemption";
        public const string COMPONENTMENU_SHADER_PROPERTY_SYNC = COMPONENTMENU_SHADER_REPLACEMENT_FOLDER + "STS-Shader Property Sync";
        public const string COMPONENTMENU_SHADER_REPLACEMENT_MAPPINGS = COMPONENTMENU_SHADER_REPLACEMENT_FOLDER + "STS-Shader Replacement Mappings";

        public const string COMPONENTMENU_TRIGGERSANDTOGGLES_FOLDER = COMPONENTMENU_FOLDER + "Triggers And Toggles/"; //FOLDER
        public const string COMPONENTMENU_TRIGGER_FOLDER_PLAYERBASED = COMPONENTMENU_TRIGGERSANDTOGGLES_FOLDER + "PlayerBased/"; //FOLDER
        public const string COMPONENTMENU_TOGGLE_FOLDER_INDEPENDENT= COMPONENTMENU_TRIGGERSANDTOGGLES_FOLDER + "Independent/"; //FOLDER

        // PlayerBased
        public const string COMPONENTMENU_BUILDING_AUTO_DETECTOR = COMPONENTMENU_TRIGGER_FOLDER_PLAYERBASED + "STS-Building Auto-Detector";
        public const string COMPONENTMENU_TRIGGER_BY_PARENT = COMPONENTMENU_TRIGGER_FOLDER_PLAYERBASED + "STS-Trigger by Parent";

        public const string COMPONENTMENU_TRIGGER_BY_ID_FOLDER = COMPONENTMENU_TRIGGER_FOLDER_PLAYERBASED + "Trigger by Id/"; //FOLDER
        public const string COMPONENTMENU_TRIGGER_BY_ID = COMPONENTMENU_TRIGGER_BY_ID_FOLDER + "STS-Trigger by Id";
        public const string COMPONENTMENU_TRIGGER_OBJECT_ID = COMPONENTMENU_TRIGGER_BY_ID_FOLDER + "STS-Trigger Object Id";

        public const string COMPONENTMENU_TRIGGER_BY_BOX_FOLDER = COMPONENTMENU_TRIGGER_FOLDER_PLAYERBASED + "Trigger by Box/"; //FOLDER
        public const string COMPONENTMENU_TRIGGER_BY_BOX = COMPONENTMENU_TRIGGER_BY_BOX_FOLDER + "STS-Trigger by Box";
        public const string COMPONENTMENU_TRIGGER_BOX = COMPONENTMENU_TRIGGER_BY_BOX_FOLDER + "STS-Trigger Box";

        // Independent
        public const string COMPONENTMENU_TOGGLE_BY_UI = COMPONENTMENU_TOGGLE_FOLDER_INDEPENDENT + "STS-Toggle by UI";
        public const string COMPONENTMENU_TOGGLE_BY_CLICK = COMPONENTMENU_TOGGLE_FOLDER_INDEPENDENT + "STS-Toggle by Click";
        public const string COMPONENTMENU_TOGGLE_BY_CLICK_ZONES_ONLY = COMPONENTMENU_TOGGLE_FOLDER_INDEPENDENT + "STS-Toggle by Click Zones Only";
        public const string COMPONENTMENU_TOGGLE_BY_UI_ZONES_ONLY = COMPONENTMENU_TOGGLE_FOLDER_INDEPENDENT + "STS-Toggle by UI Zones Only";

        public const string COMPONENTMENU_PLAYER_FOLDER = COMPONENTMENU_FOLDER + "Player/"; //FOLDER
        public const string COMPONENTMENU_PREFAB_INSTANCE = COMPONENTMENU_PLAYER_FOLDER + "STS-Prefab Instance";
        public const string COMPONENTMENU_PLAYER_POSITION_MANAGER = COMPONENTMENU_PLAYER_FOLDER + "STS-Player Position Manager";
        public const string COMPONENTMENU_PLAYER = COMPONENTMENU_PLAYER_FOLDER + "STS-Player";


        /////////////
        // TITLES: // 
        /////////////
        public static readonly string PLAYER_POSITION_MANAGER_TITLE = "Playable Characters Position Manager";
        public static readonly string GLOBAL_SHADER_REPLACEMENT_TITLE = "Global Shader Replacement Manager";
        public static readonly string GROUP_SHADER_REPLACEMENT_TITLE = "Shader Replacement By Group";
        public static readonly string STS_EXEMPTION_TITLE = "";
        public static readonly string SHADER_PROPERTY_SYNC_TITLE = "Shader Property Synchronizer";
        public static readonly string STS_ZONE = "See-through Shader Zone";

        public static readonly string SHADER_REPLACEMENT_MAPPINGS = "Shader Replacement Mappings";

        // Triggers:
        public static readonly string BUILDING_AUTO_DETECTOR_TITLE = "Building Auto-Detector";

        public static readonly string TRIGGER_BY_PARENT_TITLE = "Trigger By Parent";

        public static readonly string TRIGGER_BY_ID_TITLE = "Trigger By Id";
        public static readonly string TRIGGER_OBJECT_ID_TITLE = "Object Id For Trigger by Id";

        public static readonly string TRIGGER_BY_BOX_TITLE = "Trigger By Box";
        public static readonly string TRIGGER_BOX_TITLE = "Trigger Box";
        //Toggles:
        public static readonly string TOGGLE_BY_UI_TITLE = "Toggle By UI";
        public static readonly string TOGGLE_BY_CLICK_TITLE = "Toggle By Click";

        // DESCRIPTIONS:
        public static readonly string PLAYER_POSITION_MANAGER_DESCRIPTION = "";
        public static readonly string GLOBAL_SHADER_REPLACEMENT_DESCRIPTION = "Script for the Camera or a empty GameObject, that <b>adds the <i>See-through Shader</i>" +
                                                                             " globally to all GameObjects that are on the LayerMasks supplied</b>. \n\nIt is mostly " +
                                                                                "used to easily add the shader to all materials at once in contrast to the “SeeThroughShaderGroupReplacement” " +
                                                                                "script, which works locally from a parent GameObject downwards. " +
                                                                                "\n\nIt also synchronizes the property settings from its assigned reference material to all affected materials.";


        public static readonly string GROUP_SHADER_REPLACEMENT_DESCRIPTION = "This script <b>assigns the <i>See-through Shader</i> to elements beneath a parent.</b> " +
                                                                             "\n\nIn contrast to the “GlobalReplacementShader” script, which will add the shader globally to all GameObjects that are on the LayerMasks supplied, " +
                                                                            "this script will only apply the shader locally to the children of the GameObject, this script got assigned to. " +
                                                                            "\n\nIt also synchronizes the property settings from its assigned reference material to all affected materials.";



        public static readonly string STS_EXEMPTION_DESCRIPTION = "";
        public static readonly string SHADER_PROPERTY_SYNC_DESCRIPTION = "";

        public static readonly string STS_ZONE_DESCRIPTION = "";


        public static readonly string BUILDING_AUTO_DETECTOR_DESCRIPTION = "";

        public static readonly string TRIGGER_BY_PARENT_DESCRIPTION = "";

        public static readonly string TRIGGER_BY_ID_DESCRIPTION = "";
        public static readonly string TRIGGER_OBJECT_ID_DESCRIPTION = "";

        public static readonly string TRIGGER_BY_BOX_DESCRIPTION = "";
        public static readonly string TRIGGER_BOX_DESCRIPTION = "";

        public static readonly string TOGGLE_BY_UI_DESCRIPTION = "";
        public static readonly string TOGGLE_BY_CLICK_DESCRIPTION = "";
    }
}