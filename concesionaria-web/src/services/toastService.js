import { toast } from "sonner";
import { createElement } from "react";
import {
  CircleCheck,
  CircleX,
  Info,
  TriangleAlert,
} from "lucide-react";

const getColors = (type) => {
  switch (type) {
    case "success": return "hsl(var(--nav-bg))";
    case "error": return "#ef4444";
    case "warning": return "#f59e0b";
    case "info": return "hsl(var(--nav-bg))";
    default: return "hsl(var(--nav-bg))";
  }
};

const getSoftColor = (type) => {
  switch (type) {
    case "error": return "rgba(239, 68, 68, 0.08)";
    case "warning": return "rgba(245, 158, 11, 0.09)";
    default: return "hsl(var(--nav-bg) / 0.09)";
  }
};

const getIcon = (type) => {
  switch (type) {
    case "success": return CircleCheck;
    case "error": return CircleX;
    case "warning": return TriangleAlert;
    case "info": return Info;
    default: return Info;
  }
};

const show = (type, message, options = {}) => {
  const color = getColors(type);
  const softColor = getSoftColor(type);
  const Icon = getIcon(type);

  toast(message, {
    description: options.description,
    style: {
      background: "#ffffff",
      width: "330px",
      minHeight: "unset",
      border: "1px solid #e2e8f0",
      borderRadius: "10px",
      boxShadow: "0 8px 24px -10px rgba(15, 23, 42, 0.28)",
      padding: "9px 11px",
      gap: "12px",
    },
    icon: createElement(
      "div",
      {
        style: {
          width: "26px",
          height: "26px",
          flexShrink: 0,
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          borderRadius: "8px",
          color,
          backgroundColor: softColor,
        },
      },
      createElement(Icon, { size: 15, strokeWidth: 2.4 }),
    ),
  });
};

export const toastService = {
  success: (msg, opt) => show("success", msg, opt),
  error: (msg, opt) => show("error", msg, opt),
  warning: (msg, opt) => show("warning", msg, opt),
  info: (msg, opt) => show("info", msg, opt),
};
