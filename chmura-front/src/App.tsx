import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom"
import { projectComponents } from './components/projectComponents'
import './style/App.css'
import './style/mainStyles.css'


const router = createBrowserRouter(projectComponents.map(p => {
  return {
    path: p.path,
    element: p.element
  }
}));

function App() {

  return (
    <>
      <RouterProvider router={router} />
    </>
  )
}

export default App
