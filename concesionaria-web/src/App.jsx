import { Toaster } from "sonner";
import AppRoutes from "./routes/AppRoutes";

function App() {
  return (
    <>
      <AppRoutes />
      <Toaster
        position="bottom-left"
        offset={{ bottom: "64px", left: "16px" }}
        gap={8}
        visibleToasts={3}
        duration={3500}
        closeButton
        toastOptions={{
          classNames: {
            title: "text-[13px] font-bold text-slate-900",
            description: "text-xs leading-4 text-slate-600",
            closeButton: "border-slate-200 bg-white text-slate-400 hover:bg-slate-100 hover:text-slate-700",
          },
        }}
      />
    </>
  );
}

export default App;
