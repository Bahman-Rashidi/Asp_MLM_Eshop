/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.filebrowserImageUploadUrl = '/AdminProducts/UploadImage';

    //config.contentsLangDirection = 'rtl';

    config.language = 'en';

    config.toolbar = [
     { items: ['Templates', 'clipboard', 'Cut', 'Paste', 'Redo', 'Undo', 'Find', '-', 'basicstyles', 'cleanup', 'Link', 'Unlink', 'Anchor', 'Image', 'Smiley', 'Flash', 'Table', 'SpecialChar', 'Syntaxhighlight', '-', 'Blockquote', 'Maximize', 'Preview'] },
     { items: ['Format', 'Font', 'FontSize', '-', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript', '-', 'NumberedList', 'BulletedList', 'Indent', 'Outdent', '-', 'JustifyBlock', 'JustifyRight', 'JustifyCenter', 'JustifyLeft', 'BidiRtl', 'BidiLtr', 'TextColor', 'BGColor', 'Source'] }
    ];
};
