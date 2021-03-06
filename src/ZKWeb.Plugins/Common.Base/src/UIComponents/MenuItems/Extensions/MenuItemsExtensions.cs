﻿using Newtonsoft.Json;
using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWeb.Templating;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.MenuItems.Extensions {
	/// <summary>
	/// 菜单项列表的扩展函数
	/// </summary>
	public static class MenuItemsExtensions {
		/// <summary>
		/// 添加刷新项
		/// 可用于
		/// - ajax表格，要求在.ajax-table-menu中
		/// </summary>
		/// <param name="items">菜单项列表</param>
		public static void AddRefresh(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.refresh.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加全屏项
		/// 可用于
		/// - 全屏所在的.portlet
		/// </summary>
		/// <param name="items">菜单项列表</param>
		public static void AddFullscreen(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.fullscreen.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加分割线
		/// </summary>
		/// <param name="items">菜单项列表</param>
		public static void AddDivider(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.divider.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加对表格的操作
		/// 包含导出xls，打印
		/// </summary>
		/// <param name="items">菜单项列表</param>
		public static void AddTableOperations(this List<MenuItem> items) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate("common.base/tmpl.menu_item.table_operations.html", null);
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加每页数量设置
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="pageSizes">默认[5, 25, 50, 100, 500]</param>
		public static void AddPaginationSettings(this List<MenuItem> items, int[] pageSizes = null) {
			pageSizes = pageSizes ?? new[] { 5, 25, 50, 100, 500 };
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.menu_item.pagination_settings.html", new { pageSizes });
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加处理点击事件的菜单项
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标css类</param>
		/// <param name="onClick">点击时执行的Javascript代码</param>
		public static void AddItemForClickEvent(
			this IList<MenuItem> items, string name, string iconClass, string onClick) {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.menu_item.onclick.html",
				new { name, iconClass, onClick });
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加打开链接使用的菜单项
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标css类</param>
		/// <param name="href">链接地址</param>
		/// <param name="target">打开目标</param>
		public static void AddItemForLink(
			this IList<MenuItem> items, string name, string iconClass,
			string href, string target = "_self") {
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var html = templateManager.RenderTemplate(
				"common.base/tmpl.menu_item.link.html",
				new { name, iconClass, href, target });
			items.Add(new MenuItem(html));
		}

		/// <summary>
		/// 添加使用模态框弹出指定指定页面的菜单项
		/// 支持在数据改变后模态框关闭时刷新Ajax表格
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="title">模态框标题</param>
		/// <param name="url">远程链接</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog的参数</param>
		public static void AddRemoteModalForAjaxTable(this List<MenuItem> items,
			string name, string iconClass, string title, string url, object dialogParameters = null) {
			items.AddItemForClickEvent(name, iconClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				table.showRemoteModalForRow(null, {0}, {1}, {2});",
				JsonConvert.SerializeObject(title),
				JsonConvert.SerializeObject(url),
				JsonConvert.SerializeObject(dialogParameters)));
		}

		/// <summary>
		/// 使用模态框弹出指定指定页面的菜单项
		/// 链接模板传入Ajax表格当前选中单行的数据
		/// 支持在数据改变后模态框关闭时刷新Ajax表格
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="name">显示名称</param>
		/// <param name="iconClass">图标Css类</param>
		/// <param name="titleTemplate">模态框标题的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="urlTemplate">远程链接的模板，格式是underscore.js的默认格式，参数传入row</param>
		/// <param name="dialogParameters">用于覆盖传入给BootstrapDialog.show的参数</param>
		public static void AddRemoteModalForSelectedRow(
			this IList<MenuItem> items, string name, string iconClass,
			string titleTemplate, string urlTemplate, object dialogParameters = null) {
			items.AddItemForClickEvent(name, iconClass, string.Format(@"
				var table = $(this).closestAjaxTable();
				var row = table.getSingleSelectedRowData();
				row && table.showRemoteModalForRow(row, {0}, {1}, {2});",
				JsonConvert.SerializeObject(titleTemplate),
				JsonConvert.SerializeObject(urlTemplate),
				JsonConvert.SerializeObject(dialogParameters)));
		}

		/// <summary>
		/// Ajax表格树展开/折叠全部的菜单项
		/// </summary>
		/// <param name="items">菜单项列表</param>
		/// <param name="levelMember">保存节点等级的成员名称</param>
		public static void AddToggleAllForAjaxTableTree(
			this IList<MenuItem> items, string levelMember) {
			items.AddItemForClickEvent(new T("Expand/Collapse All"), "fa fa-expand",
				string.Format("$(this).closestAjaxTable().treeNodeToggleAll({0})",
				JsonConvert.SerializeObject(levelMember)));
		}
	}
}
