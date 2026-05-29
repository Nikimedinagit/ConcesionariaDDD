import { toast } from "sonner";
import { createElement } from "react";

const getColors = (type) => {
  switch (type) {
    case "success": return "#22c55e";
    case "error": return "#ef4444";
    case "warning": return "#f59e0b";
    case "info": return "#3b82f6";
    default: return "#3b82f6";
  }
};

const show = (type, message, options = {}) => {
  const color = getColors(type);

  toast(message, {
    description: options.description,
    style: {
      background: "#ffffff",
      borderRadius: "12px",
      boxShadow: "0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)",
      padding: "16px",
    },
    icon: createElement("div", { 
      style: { 
        width: '14px', 
        height: '14px', 
        borderRadius: '50%', 
        backgroundColor: color 
      } 
    }),
  });
};

export const toastService = {
  success: (msg, opt) => show("success", msg, opt),
  error: (msg, opt) => show("error", msg, opt),
  warning: (msg, opt) => show("warning", msg, opt),
  info: (msg, opt) => show("info", msg, opt),
};