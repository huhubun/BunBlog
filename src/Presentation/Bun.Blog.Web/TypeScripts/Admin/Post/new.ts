﻿/// <reference path="../../../typings/index.d.ts" />
/// <reference path="../../common/enums.ts" />

import Enums = Bun.Blog.Common.Enums;

namespace Bun.Blog.Admin.Post {
    export class NewOptions {
        publishUrl: string;
    }

    export class New {
        options: NewOptions;

        public constructor(options: NewOptions) {
            this.options = options;
            this.init();
        }

        private init() {
            $("#EditorToolbar").kendoToolBar({
                items: [
                    {
                        type: "buttonGroup",
                        buttons: [
                            { text: "文本", togglable: true, group: "position", selected: true },
                            { text: "预览", togglable: true, group: "position" }
                        ]
                    },
                    { type: "separator" },
                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-bold"></span></button>` },
                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-italic"></span></button>` },

                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-quote-left"></span></button>` },
                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-code"></span></button>` },
                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-link"></span></button>` },

                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-list"></span></button>` },
                    { template: `<button class="bun-editor-toolbar-btn" type="button"><span class="fa fa-list-ol"></span></button>` },
                ]
            });

            $("#BreadcrumbMenu .publish-post").click(e => {
                var publishData = {
                    title: $("#Title").val(),
                    content: $("#Content").val()
                };

                var n = $("#Notifications").kendoNotification({
                    appendTo: "#Notifications",
                    hideOnClick: true,
                    autoHideAfter: 0,
                    templates: [{
                        type: "success",
                        template: $("#KendoSuccessNotificationTemplate").html()
                    }, {
                        type: "error",
                        template: $("#KendoErrorNotificationTemplate").html()
                    }]
                }).data("kendoNotification");

                $.post(this.options.publishUrl, publishData, (data) => {
                    if (data.status == Enums.JsonResponseStatus.success) {
                        location.href = data.content.editPostUrl;
                    }
                    else {

                        n.error("文章保存失败 " + data.message);
                    }
                }).fail((xhr, status, errorThrown) => { n.error("文章保存失败 " + errorThrown); });


            });

        }
    }
}