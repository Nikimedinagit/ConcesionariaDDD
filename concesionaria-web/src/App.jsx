import { Toaster } from "sonner";
import AppRoutes from "./routes/AppRoutes";

function App() {
  return (
    <>
      <AppRoutes />
      <Toaster
        position="bottom-left"
        offset="16px" 
        toastOptions={{
          style: {
            bottom : "42px", 
          },
        }}
      />
    </>
  );
}

export default App;
