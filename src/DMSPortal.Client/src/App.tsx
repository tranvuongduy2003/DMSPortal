import { AuthProtectedRoute, HomeLayout } from '@/layouts'
import { LoginPage, ResetPasswordPage, ForgotPasswordPage } from '@/pages/auth'
import { NotFoundPage } from '@/pages/notfound'
import { Route, Routes } from 'react-router-dom'

function App() {
  return (
    <Routes>
      <Route>
        {/* Auth Route */}
        <Route element={<AuthProtectedRoute />}>
          <Route path='/auth/login' element={<LoginPage />} />
          <Route path='/auth/reset-password' element={<ResetPasswordPage />} />
          <Route path='/auth/forgot-password' element={<ForgotPasswordPage />} />
        </Route>

        <Route path='' element={<HomeLayout />}></Route>

        <Route path='*' element={<NotFoundPage />} />
      </Route>
    </Routes>
  )
}

export default App
