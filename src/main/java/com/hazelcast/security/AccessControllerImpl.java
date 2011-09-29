package com.hazelcast.security;

import java.security.AccessControlException;
import java.security.Permission;
import java.security.PermissionCollection;
import java.security.PrivilegedAction;
import java.util.logging.Level;

import javax.security.auth.Subject;

import com.hazelcast.impl.CallContext;
import com.hazelcast.impl.ThreadContext;
import com.hazelcast.logging.ILogger;
import com.hazelcast.logging.Logger;

public class AccessControllerImpl implements IAccessController {
	
	private final ILogger logger = Logger.getLogger(IAccessController.class.getName());
	
	private final IClusterPolicy policy;
	
	public AccessControllerImpl(IClusterPolicy policy) {
		super();
		this.policy = policy;
	}
	
	public void checkPermission(Permission permission) throws AccessControlException {
		final CallContext ctx = ThreadContext.get().getCallContext();
		final Subject subject = ctx.getSubject();
		if(subject == null) {
			throw new AccessControlException("Unauthorized access!", permission);
		}
		if(!checkPermission(subject, permission)) {
			throw new AccessControlException("Permission " + permission + " denied!", permission);
		}
	}
	
	public boolean checkPermission(Subject subject, Permission permission) {
		PermissionCollection coll = policy.getPermissions(subject, permission);
		return coll != null ? coll.implies(permission) : false;
	}

	public <T> T doAsPrivileged(Subject subject, PrivilegedAction<T> action) throws AccessControlException {
		final CallContext ctx = ThreadContext.get().getCallContext();
		Subject s = ctx.getSubject();
		if(s != null && !s.equals(subject)) {
			logger.log(Level.WARNING, "Here is another subject bound into context before!");
		}
		ctx.setSubject(subject);
		try {
			return action.run();
		} finally {
			ctx.setSubject(null);
		}
	}
}
